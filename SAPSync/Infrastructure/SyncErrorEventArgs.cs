using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public class SyncErrorEventArgs : EventArgs
    {
        public enum ErrorSeverity
        {
            Minor,
            Major,
            Critical
        }
        #region Properties

        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public string NameOfElement { get; set; }
        public Type TypeOfElement { get; set; }
        public SyncProgress Progress { get; set; }
        public ErrorSeverity Severity { get; set; }
        public DateTime TimeStamp { get; set; }

        #endregion Properties
    }
}
