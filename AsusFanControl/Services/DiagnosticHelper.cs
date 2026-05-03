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
        public bool AsusServiceFound { get; set; }
        public bool AsusServiceRunning { get; set; }
        public List<string> AsusServicesFound { get; set; }
        public List<string> Errors { get; set; }

        public DiagnosticResult()
        {
            FanRpms = new List<int>();
            AsusServicesFound = new List<string>();
            Errors = new List<string>();
        }

        public bool AllPassed => IsAdmin && DllExists && DllLoaded && CanReadFanCount && FanCount > 0 && CanReadTemp && CpuTemp > 0 && CanReadRpm;

        public string GetSummary()
        {
            var lines = new List<string>();
            lines.Add($"Admin privileges:     {(IsAdmin ? "YES" : "NO -- Run as Administrator!")}");
            lines.Add($"AsusWinIO64.dll:      {(DllExists ? "Found" : "MISSING -- DLL not next to exe!")}");
            lines.Add($"DLL loaded:           {(DllLoaded ? "YES" : "NO -- InitializeWinIo failed!")}");

            if (AsusServicesFound.Count > 0)
            {
                lines.Add($"ASUS services found:  {string.Join(", ", AsusServicesFound)}");
                lines.Add($"ASUS service running: {(AsusServiceRunning ? "YES" : "NO -- Start it in Windows Services!")}");
            }
            else
            {
                lines.Add($"ASUS services found:  NONE FOUND");
                lines.Add($"");
                lines.Add($"  ** ASUS System Control Interface is NOT installed. **");
                lines.Add($"  Install MyASUS from the Microsoft Store, then");
                lines.Add($"  ensure 'ASUS System Analysis' service is running.");
            }

            lines.Add($"");
            lines.Add($"Fan count:            {FanCount}{(FanCount <= 0 ? "  ** INVALID - service not working **" : "")}");
            lines.Add($"CPU temperature:      {CpuTemp} C{(CpuTemp == 0 ? "  ** INVALID - service not working **" : "")}");

            if (CanReadRpm && FanRpms.Count > 0)
                lines.Add($"Fan RPM:              {string.Join(", ", FanRpms)}");
            else
                lines.Add($"Fan RPM:              FAILED (fan count was {FanCount})");

            if (Errors.Count > 0)
            {
                lines.Add($"");
                lines.Add($"Errors:");
                foreach (var err in Errors)
                    lines.Add($"  - {err}");
            }

            lines.Add($"");
            if (AllPassed)
                lines.Add("Overall: ALL CHECKS PASSED");
            else if (FanCount <= 0 || CpuTemp == 0)
                lines.Add("Overall: ASUS SYSTEM CONTROL INTERFACE NOT WORKING\n\nThe DLL loaded but cannot communicate with hardware.\nMake sure the ASUS System Analysis service is running.\n\nSteps:\n1. Install MyASUS from Microsoft Store\n2. Open Windows Services (services.msc)\n3. Find 'ASUS System Analysis' and Start it\n4. Restart this application");
            else
                lines.Add("Overall: SOME CHECKS FAILED -- see above");

            return string.Join(Environment.NewLine, lines);
        }
    }

    public static class DiagnosticHelper
    {
        // Known ASUS service name patterns
        private static readonly string[] AsusServicePatterns = new[]
        {
            "asussci",
            "asussystem",
            "aborhelper",
            "aaborhelper",
            "asusoptimization",
            "asuslinkclient",
            "asuslinknear",
            "asussoftwaremanager",
            "asusappservice",
            "aaborhelperservice"
        };

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

        public static void FindAsusServices(out List<string> found, out bool anyRunning)
        {
            found = new List<string>();
            anyRunning = false;

            try
            {
                var services = ServiceController.GetServices();
                foreach (var svc in services)
                {
                    var nameLower = svc.ServiceName.ToLowerInvariant();
                    var displayLower = svc.DisplayName.ToLowerInvariant();

                    bool isAsus = nameLower.Contains("asus") || displayLower.Contains("asus");

                    if (!isAsus)
                    {
                        foreach (var pattern in AsusServicePatterns)
                        {
                            if (nameLower.Contains(pattern))
                            {
                                isAsus = true;
                                break;
                            }
                        }
                    }

                    if (isAsus)
                    {
                        var status = svc.Status == ServiceControllerStatus.Running ? "Running" : svc.Status.ToString();
                        found.Add($"{svc.ServiceName} ({status})");

                        if (svc.Status == ServiceControllerStatus.Running)
                            anyRunning = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log("FindAsusServices", ex);
            }
        }

        public static DiagnosticResult RunFullDiagnostic(AsusControl asusControl)
        {
            var result = new DiagnosticResult();

            // Check admin
            result.IsAdmin = IsRunAsAdmin();

            // Check DLL
            result.DllExists = DllExistsNextToExe();

            // Check ASUS services
            List<string> found;
            bool anyRunning;
            FindAsusServices(out found, out anyRunning);
            result.AsusServicesFound = found;
            result.AsusServiceFound = found.Count > 0;
            result.AsusServiceRunning = anyRunning;

            // Check DLL loaded
            result.DllLoaded = asusControl != null;

            if (asusControl == null)
            {
                result.Errors.Add("AsusControl is null -- DLL failed to initialize.");
                return result;
            }

            // Check fan count
            try
            {
                result.FanCount = asusControl.HealthyTable_FanCounts();
                result.CanReadFanCount = true;

                if (result.FanCount <= 0)
                    result.Errors.Add($"FanCounts returned {result.FanCount}. ASUS service may not be running or communicating.");
            }
            catch (Exception ex)
            {
                result.Errors.Add($"FanCounts exception: {ex.Message}");
                ErrorLogger.Log("Diagnostic.FanCount", ex);
            }

            // Check CPU temp
            try
            {
                result.CpuTemp = (int)asusControl.Thermal_Read_Cpu_Temperature();
                result.CanReadTemp = true;

                if (result.CpuTemp == 0)
                    result.Errors.Add("CPU temp returned 0. ASUS service may not be running.");
            }
            catch (Exception ex)
            {
                result.Errors.Add($"ReadTemp exception: {ex.Message}");
                ErrorLogger.Log("Diagnostic.ReadTemp", ex);
            }

            // Check RPM (only if fan count is valid)
            if (result.FanCount > 0)
            {
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
                    result.Errors.Add($"GetFanSpeed exception: {ex.Message}");
                    ErrorLogger.Log("Diagnostic.GetRPM", ex);
                }
            }

            return result;
        }
    }
}
