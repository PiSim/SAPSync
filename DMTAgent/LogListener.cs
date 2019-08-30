using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Targets;
using NLog.Config;
using NLog;
using NLog.Common;

namespace DMTAgent
{
    [Target("Listener")]
    public class LogListener : Target
    {
        
        public class LogCreatedEventArgs : EventArgs
        {
            public LogCreatedEventArgs(LogEventInfo eventInfo)
            {
                LogEventInfo = eventInfo;
            }

            public LogEventInfo LogEventInfo { get; }
        }
        

        public event EventHandler<LogCreatedEventArgs> LogCreated;

        protected override void Write(LogEventInfo logEvent)
        {
            RaiseLogCreated(logEvent);
        }

        protected virtual void RaiseLogCreated(LogEventInfo info) => LogCreated?.Invoke(this, new LogCreatedEventArgs(info));

    }


}
