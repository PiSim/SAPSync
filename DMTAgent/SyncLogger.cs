using DMTAgent.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DMTAgent
{
    public class SyncLogger : ILogger
    {
        #region Fields

        private readonly AppSettings _settings;

        #endregion Fields

        #region Constructors

        public SyncLogger(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        #endregion Constructors

        #region Events

        public event EventHandler LogEntryCreated;

        #endregion Events

        #region Properties

        public LinkedList<string> CurrentLog { get; } = new LinkedList<string>();

        #endregion Properties

        #region Methods

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public void LogElementCompleted(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Elemento completato: " + element.Name
                });
        }

        public void LogElementStarting(ISyncElement element)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Avvio elemento: " + element.Name
                });
        }

        public void LogSyncError(SyncErrorEventArgs errorEventArgs)
        {
            string[] logLines = new string[]
            {
                GetTimeStamp() + errorEventArgs.Severity.ToString() + " Error: " + errorEventArgs.ErrorMessage,
                "Elemento: " + errorEventArgs.NameOfElement + " - " + errorEventArgs.TypeOfElement,
                "Eccezione: " + errorEventArgs.Exception?.Message,
            };

            CreateLogEntry(logLines, new FileInfo(_settings.ErrorLogPath));
        }

        public void LogTaskCompleted(IJob task)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Sincronizzazione completata: "
                });
        }

        public void LogTaskScheduled(DateTime time)
        {
            NewLogEntry(new string[] { GetTimeStamp() + "Schedulata sincronizzazione: " + time.ToString("dd/MM/yyyy HH:mm") });
        }

        public void LogTaskStarting(IJob task)
        {
            NewLogEntry(
                new string[]
                {
                    GetTimeStamp() + "Inizio nuova sincronizzazione: "
                });
        }

        public void NewLogEntry(string[] text)
        {
            CreateLogEntry(text, new FileInfo(_settings.GeneralLogPath));
        }

        public void RaiseLogEntryCreated() => LogEntryCreated?.Invoke(null, new EventArgs());

        private void CreateLogEntry(IEnumerable<string> lines, FileInfo target)
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

        private string GetTimeStamp() => DateTime.Now.ToString("yyyyMMddhhmmss_");

        #endregion Methods
    }
}