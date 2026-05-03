using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.ServiceProcess;

namespace AsusFanControl.Services
{
    public class DiagnosticResult
    {
        public bool IsAdmin { get; set; }
        public bool DllExists { get; set; }
        public bool DllLoaded { get; set; }
        public bool CanReadFanCount { get; set; }
        public int FanCount { get; set; }
        public bool CanReadTemp { get; set; }
        public int CpuTemp { get; set; }
        public bool CanReadRpm { get; set; }
        public List<int> FanRpms { get; set; }
        public bool AsusServiceRunning { get; set; }
        public string Error { get; set; }

        public bool AllPassed => IsAdmin && DllExists && DllLoaded && CanReadFanCount && FanCount > 0 && CanReadTemp && CanReadRpm;

        public string GetSummary()
        {
            var lines = new List<string>();
            lines.Add($"Admin privileges:     {(IsAdmin ? "YES" : "NO -- Run as Administrator!")}");
            lines.Add($"AsusWinIO64.dll:      {(DllExists ? "Found" : "MISSING -- DLL not next to exe!")}");
            lines.Add($"ASUS service:         {(AsusServiceRunning ? "Running" : "Not detected (may still work)")}");
            lines.Add($"DLL loaded:           {(DllLoaded ? "YES" : "NO -- InitializeWinIo failed!")}");
            lines.Add($"Fan count:            {(CanReadFanCount ? FanCount.ToString() : "FAILED")}");
            lines.Add($"CPU temperature:      {(CanReadTemp ? $"{CpuTemp} C" : "FAILED")}");
            lines.Add($"Fan RPM:              {(CanReadRpm ? string.Join(", ", FanRpms) : "FAILED")}");

            if (!string.IsNullOrEmpty(Error))
                lines.Add($"\nError details: {Error}");

            lines.Add($"\nOverall: {(AllPassed ? "ALL CHECKS PASSED" : "SOME CHECKS FAILED -- see above")}");

            return string.Join(Environment.NewLine, lines);
        }
    }

    public static class DiagnosticHelper
    {
        public static bool IsRunAsAdmin()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        public static bool DllExistsNextToExe()
        {
            try
            {
                var exeDir = AppDomain.CurrentDomain.BaseDirectory;
                var dllPath = Path.Combine(exeDir, "AsusWinIO64.dll");
                return File.Exists(dllPath);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAsusServiceRunning()
        {
            try
            {
                var services = ServiceController.GetServices();
                foreach (var svc in services)
                {
                    if (svc.ServiceName.IndexOf("ASUSSystem", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        svc.ServiceName.IndexOf("AsusSystemAnalysis", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return svc.Status == ServiceControllerStatus.Running;
                    }
                }
            }
            catch { }
            return false;
        }

        public static DiagnosticResult RunFullDiagnostic(AsusControl asusControl)
        {
            var result = new DiagnosticResult
            {
                FanRpms = new List<int>()
            };

            // Check admin
            result.IsAdmin = IsRunAsAdmin();

            // Check DLL
            result.DllExists = DllExistsNextToExe();

            // Check ASUS service
            result.AsusServiceRunning = IsAsusServiceRunning();

            // Check DLL loaded (if asusControl was created, InitializeWinIo was called)
            result.DllLoaded = asusControl != null;

            if (asusControl == null)
            {
                result.Error = "AsusControl is null -- DLL failed to initialize.";
                return result;
            }

            // Check fan count
            try
            {
                result.FanCount = asusControl.HealthyTable_FanCounts();
                result.CanReadFanCount = true;
            }
            catch (Exception ex)
            {
                result.Error = $"FanCounts failed: {ex.Message}";
                ErrorLogger.Log("Diagnostic.FanCount", ex);
            }

            // Check CPU temp
            try
            {
                result.CpuTemp = (int)asusControl.Thermal_Read_Cpu_Temperature();
                result.CanReadTemp = true;
            }
            catch (Exception ex)
            {
                result.Error = $"ReadTemp failed: {ex.Message}";
                ErrorLogger.Log("Diagnostic.ReadTemp", ex);
            }

            // Check RPM
            try
            {
                for (byte i = 0; i < result.FanCount; i++)
                {
                    result.FanRpms.Add(asusControl.GetFanSpeed(i));
                }
                result.CanReadRpm = result.FanRpms.Count > 0;
            }
            catch (Exception ex)
            {
                result.Error = $"GetFanSpeed failed: {ex.Message}";
                ErrorLogger.Log("Diagnostic.GetRPM", ex);
            }

            return result;
        }
    }
}
