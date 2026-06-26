using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StratusBot
{
    public class LogEntry
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

    }
    public class ActivityLog
    {
        private readonly string _logFilePath;
        private const string LogFileName = "activity_log.json";

        public ActivityLog(string? logDirectory = null)
        {
            //logs directory in AppData
            //string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StratusBot");

            // if caller doesn't supply a path, use the app folder (portable)
            string appDir = logDirectory ?? AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(appDir);
            _logFilePath = Path.Combine(appDir, LogFileName);
        }

        //logs an action with the timestamp
        public void LogAction(string action)
        {
            LogAction("INFO", action);
        }

        //log an action with the a specific category
        public void LogAction(string category, string action)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"),
                    Category = category,
                    Action = action
                };
                var logs = GetLogsFromFile();
                logs.Add(logEntry);
                SaveLogsToFile(logs);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging action: {ex.Message}");
            }
        }

        //Retrieve all log entries
        public List<LogEntry> GetAllLogs()
        {
            try
            {
                return GetLogsFromFile();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading logs: {ex.Message}");
                return new List<LogEntry>();
            }
        }

        //Retrieves the latest log entries
        public List<LogEntry> GetLastLogs(int count)
        {
            try
            {
                var allLogs = GetLogsFromFile();
                return allLogs.Skip(Math.Max(0,allLogs.Count - count)).ToList();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading logs: {ex.Message}");
                return new List<LogEntry>();
            }
        }

        //Retrieves logs filtred by category
        public List<LogEntry> GetLogsByCategory(string category)
        {
            try
            {
                var allLogs = GetLogsFromFile();
                return allLogs.Where(log => log.Category == category).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading logs: {ex.Message}");
                return new List<LogEntry>();
            }
        }

        //Clears all log entries
        public void ClearLogs()
        {
            try
            {
                if (File.Exists(_logFilePath))
                    File.Delete(_logFilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing logs: {ex.Message}");
            }
        }

        //get the log file path
        public string GetLogFilePath()
        {
            return _logFilePath;
        }

        private List<LogEntry> GetLogsFromFile()
        {
            try
            {
                if (!File.Exists(_logFilePath))
                    return new List<LogEntry>();

                string jsonString = File.ReadAllText(_logFilePath);
                return JsonConvert.DeserializeObject<List<LogEntry>>(jsonString) ?? new List<LogEntry>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing logs: {ex.Message}");
                return new List<LogEntry>();
            }
        }

        private void SaveLogsToFile(List<LogEntry> logs)
        {
            try
            {
                string json = JsonConvert.SerializeObject(logs, Formatting.Indented);
                File.WriteAllText(_logFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving logs: {ex.Message}");
            }
        }

    }
}
