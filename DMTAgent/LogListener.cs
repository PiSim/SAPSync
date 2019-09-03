using NLog;
using NLog.Targets;
using System;

namespace DMTAgent
{
    [Target("Listener")]
    public class LogListener : Target
    {
        #region Events

        public event EventHandler<LogCreatedEventArgs> LogCreated;

        #endregion Events

        #region Methods

        protected virtual void RaiseLogCreated(LogEventInfo info) => LogCreated?.Invoke(this, new LogCreatedEventArgs(info));

        protected override void Write(LogEventInfo logEvent)
        {
            RaiseLogCreated(logEvent);
        }

        #endregion Methods

        #region Classes

        public class LogCreatedEventArgs : EventArgs
        {
            #region Constructors

            public LogCreatedEventArgs(LogEventInfo eventInfo)
            {
                LogEventInfo = eventInfo;
            }

            #endregion Constructors

            #region Properties

            public LogEventInfo LogEventInfo { get; }

            #endregion Properties
        }

        #endregion Classes
    }
}