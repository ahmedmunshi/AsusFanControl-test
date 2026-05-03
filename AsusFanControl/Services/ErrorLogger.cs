using System;
using System.IO;

namespace AsusFanControl.Services
{
    public static class ErrorLogger
    {
        private static readonly string _logDir;
        private static readonly string _logFile;
        private static readonly object _lock = new object();

        static ErrorLogger()
        {
            _logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AsusFanControl"
            );

            if (!Directory.Exists(_logDir))
                Directory.CreateDirectory(_logDir);

            _logFile = Path.Combine(_logDir, "error.log");
        }

        public static string LogFilePath => _logFile;

        public static void Log(string message)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_logFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
                }
            }
            catch { }
        }

        public static void Log(string context, Exception ex)
        {
            Log($"[{context}] {ex.GetType().Name}: {ex.Message}{Environment.NewLine}    StackTrace: {ex.StackTrace}");
        }

        public static void Clear()
        {
            try
            {
                lock (_lock)
                {
                    if (File.Exists(_logFile))
                        File.Delete(_logFile);
                }
            }
            catch { }
        }
    }
}
