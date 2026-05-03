using AsusFanControl.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AsusFanControl.Services
{
    public class FanCurveTickEventArgs : EventArgs
    {
        public int Temperature { get; set; }
        public Dictionary<int, int> AppliedSpeeds { get; set; } // fanIndex -> speed%
        public List<int> FanRpms { get; set; }
        public string Error { get; set; }
    }

    public class FanCurveEngine : IDisposable
    {
        private readonly AsusControl _asusControl;
        private Timer _timer;
        private FanProfile _activeProfile;
        private readonly object _lock = new object();
        private Dictionary<int, int> _lastAppliedSpeeds = new Dictionary<int, int>();
        private bool _running;
        private bool _disposed;

        private const int POLL_INTERVAL_MS = 1500;
        private const int HYSTERESIS_THRESHOLD = 2; // percent

        public event EventHandler<FanCurveTickEventArgs> OnTick;

        public bool IsRunning => _running;

        public FanCurveEngine(AsusControl asusControl)
        {
            _asusControl = asusControl ?? throw new ArgumentNullException(nameof(asusControl));
        }

        /// <summary>
        /// Starts the fan curve engine with the given profile.
        /// </summary>
        public void Start(FanProfile profile)
        {
            lock (_lock)
            {
                _activeProfile = profile ?? throw new ArgumentNullException(nameof(profile));
                _lastAppliedSpeeds.Clear();

                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                _timer = new Timer(Tick, null, 0, POLL_INTERVAL_MS);
                _running = true;

                ErrorLogger.Log($"Engine started. Mode={profile.Mode}, Profile={profile.Name}");
            }
        }

        /// <summary>
        /// Stops the fan curve engine.
        /// </summary>
        public void Stop(bool resetFans = false)
        {
            lock (_lock)
            {
                _running = false;

                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                _lastAppliedSpeeds.Clear();

                if (resetFans)
                {
                    try
                    {
                        // Reset each fan individually (synchronous) instead of async void SetFanSpeeds
                        var fanCount = _asusControl.HealthyTable_FanCounts();
                        for (byte i = 0; i < fanCount; i++)
                        {
                            _asusControl.SetFanSpeed((byte)0, i);
                            System.Threading.Thread.Sleep(20);
                        }
                        ErrorLogger.Log("Fans reset to BIOS control.");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log("Stop.ResetFans", ex);
                    }
                }

                ErrorLogger.Log("Engine stopped.");
            }
        }

        /// <summary>
        /// Updates the active profile without restarting the engine.
        /// </summary>
        public void UpdateProfile(FanProfile profile)
        {
            lock (_lock)
            {
                _activeProfile = profile;
            }
        }

        private void Tick(object state)
        {
            if (!_running)
                return;

            string error = null;

            try
            {
                FanProfile profile;
                lock (_lock)
                {
                    if (!_running || _activeProfile == null)
                        return;
                    profile = _activeProfile;
                }

                int temperature;
                try
                {
                    temperature = (int)_asusControl.Thermal_Read_Cpu_Temperature();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log("Tick.ReadTemp", ex);
                    error = $"Failed to read CPU temp: {ex.Message}";
                    FireErrorTick(error);
                    return;
                }

                int fanCount;
                try
                {
                    fanCount = _asusControl.HealthyTable_FanCounts();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log("Tick.FanCount", ex);
                    error = $"Failed to get fan count: {ex.Message}";
                    FireErrorTick(error);
                    return;
                }

                if (fanCount <= 0)
                {
                    error = $"Fan count = {fanCount}. ASUS System Analysis service not running. Open services.msc and start it.";
                    ErrorLogger.Log(error);
                    FireErrorTick(error);
                    return;
                }

                if (temperature <= 0)
                {
                    error = $"CPU temp = {temperature}. ASUS service not communicating. Restart the ASUS System Analysis service.";
                    ErrorLogger.Log(error);
                    FireErrorTick(error);
                    return;
                }

                var appliedSpeeds = new Dictionary<int, int>();
                var fanRpms = new List<int>();

                if (profile.Mode == "fixed")
                {
                    var speed = profile.FixedSpeedPercent;

                    for (int i = 0; i < fanCount; i++)
                    {
                        try
                        {
                            if (ShouldApplySpeed(i, speed))
                            {
                                _asusControl.SetFanSpeed(speed, (byte)i);
                                _lastAppliedSpeeds[i] = speed;
                            }
                            appliedSpeeds[i] = speed;
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.Log($"Tick.SetFanSpeed.Fixed[{i}]", ex);
                            error = $"Failed to set fan {i} speed: {ex.Message}";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < fanCount; i++)
                    {
                        try
                        {
                            var curve = profile.GetCurveForFan(i);
                            var targetSpeed = curve.Interpolate(temperature);

                            if (ShouldApplySpeed(i, targetSpeed))
                            {
                                _asusControl.SetFanSpeed(targetSpeed, (byte)i);
                                _lastAppliedSpeeds[i] = targetSpeed;
                            }
                            appliedSpeeds[i] = targetSpeed;
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.Log($"Tick.SetFanSpeed.Curve[{i}]", ex);
                            error = $"Failed to set fan {i} speed: {ex.Message}";
                        }
                    }
                }

                // Read current RPMs
                for (int i = 0; i < fanCount; i++)
                {
                    try
                    {
                        fanRpms.Add(_asusControl.GetFanSpeed((byte)i));
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.Log($"Tick.GetFanRPM[{i}]", ex);
                        fanRpms.Add(0);
                    }
                }

                // Fire event for GUI updates
                OnTick?.Invoke(this, new FanCurveTickEventArgs
                {
                    Temperature = temperature,
                    AppliedSpeeds = appliedSpeeds,
                    FanRpms = fanRpms,
                    Error = error
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("Tick.Unhandled", ex);
                FireErrorTick($"Unhandled error: {ex.Message}");
            }
        }

        private void FireErrorTick(string error)
        {
            OnTick?.Invoke(this, new FanCurveTickEventArgs
            {
                Temperature = 0,
                AppliedSpeeds = new Dictionary<int, int>(),
                FanRpms = new List<int>(),
                Error = error
            });
        }

        /// <summary>
        /// Returns true if the speed should be applied (hysteresis check).
        /// </summary>
        private bool ShouldApplySpeed(int fanIndex, int targetSpeed)
        {
            if (!_lastAppliedSpeeds.ContainsKey(fanIndex))
                return true;

            return Math.Abs(_lastAppliedSpeeds[fanIndex] - targetSpeed) >= HYSTERESIS_THRESHOLD;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop(false);
                _disposed = true;
            }
        }
    }
}
