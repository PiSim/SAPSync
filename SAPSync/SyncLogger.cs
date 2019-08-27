﻿using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAPSync
{
    public static class SyncLogger
    {
        #region Properties

        public static LinkedList<string> CurrentLog { get; } = new LinkedList<string>();
        public static event EventHandler LogEntryCreated;

        #endregion Properties

        #region Methods

        public static void LogElementCompleted(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Elemento completato: " + element.Name
                });
        }

        public static void RaiseLogEntryCreated() => LogEntryCreated?.Invoke(null, new EventArgs());

        public static void LogElementStarting(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Avvio elemento: " + element.Name
                });
        }

        public static void LogSyncError(SyncErrorEventArgs errorEventArgs)
        {
            string[] logLines = new string[]
            {
                GetTimeStamp() + errorEventArgs.Severity.ToString() + " Error: " + errorEventArgs.ErrorMessage,
                "Elemento: " + errorEventArgs.NameOfElement + " - " + errorEventArgs.TypeOfElement,
                "Eccezione: " + errorEventArgs.Exception?.Message,
            };

            CreateLogEntry(logLines, new FileInfo(Properties.Settings.Default.ErrorLogPath));
        }

        public static void LogTaskCompleted(IJob task)
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

        public static void LogTaskStarting(IJob task)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Inizio nuova sincronizzazione: "
                });
        }

        public static void NewLogEntry(string[] text)
        {
            CreateLogEntry(text, new FileInfo(Properties.Settings.Default.GeneralLogPath));
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
            finally
            {
                CurrentLog.AddFirst(string.Concat(lines.Select(l => l + "\n")));
                RaiseLogEntryCreated();
            }
        }

        private static string GetTimeStamp() => DateTime.Now.ToString("yyyyMMddhhmmss_");

        #endregion Methods
    }
}