using SAPSync;
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
        private static string GetTimeStamp() => DateTime.Now.ToString("yyyyMMddhhmmss_");

        public static void LogSyncError(SyncErrorEventArgs errorEventArgs)
        {
            string[] logLines = new string[]
            {
                GetTimeStamp() + errorEventArgs.Severity.ToString() + " Error: " + errorEventArgs.ErrorMessage,
                "Elemento: " + errorEventArgs.NameOfElement + " - " + errorEventArgs.TypeOfElement,
                "Fase: " + errorEventArgs.Progress,
                "Eccezione: " + errorEventArgs.Exception?.Message,
            };

            CreateLogEntry(logLines, new FileInfo(Properties.Settings.Default.ErrorLogPath));
        }

        public static void LogElementStarting(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Avvio elemento: " + element.Name
                });
        }

        public static void LogElementCompleted(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Elemento completato: " + element.Name + "\tRisultato: " + element.ElementStatus
                });
        }

        public static void LogTaskStarting(ISyncTask task)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Inizio nuova sincronizzazione: "            
                });
        }

        public static void LogTaskCompleted(ISyncTask task)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Sincronizzazione completata: "
                });
        }

        public static void LogTaskScheduled(DateTime time)
        {
            NewLogEntry(new string[] { GetTimeStamp() + "Schedulata sincronizzazione: " + time.ToString("dd/MM/yyyy HH:mm") });
        }

        public static void NewLogEntry(string[] text)
        {
            CreateLogEntry(text , new FileInfo(Properties.Settings.Default.GeneralLogPath));
        }

        private static void CreateLogEntry(IEnumerable<string> lines, FileInfo target)
        {
            try
            {
                File.AppendAllLines(target.FullName, lines);
            }
            catch
            {
                
            }
        }
    }
}
