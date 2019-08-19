using SyncService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public static class SyncLogger
    {
        public static void LogSyncError(SyncErrorEventArgs errorEventArgs)
        {
            string[] logLines = new string[]
            {
                errorEventArgs.TimeStamp.ToString("yyyyMMddhhmmss_") + errorEventArgs.Severity.ToString() + " Error: " + errorEventArgs.ErrorMessage,
                "Elemento: " + errorEventArgs.NameOfElement + " - " + errorEventArgs.TypeOfElement,
                "Fase: " + errorEventArgs.Progress,
                "Eccezione: " + errorEventArgs.Exception?.Message,
            };

            CreateLogEntry(logLines);
        }

        public static void CreateLogEntry(IEnumerable<string> lines)
        {
            string logPath = Properties.Settings.Default.LogFilePath;
            File.AppendAllLines(logPath, lines);
        }
    }
}
