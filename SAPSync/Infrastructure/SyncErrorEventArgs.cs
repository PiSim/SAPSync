using System;

namespace SAPSync.Infrastructure
{
    public class SyncErrorEventArgs : EventArgs
    {
        #region Enums

        public enum ErrorSeverity
        {
            Minor,
            Major,
            Critical
        }

        #endregion Enums

        #region Properties

        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public string NameOfElement { get; set; }
        public ErrorSeverity Severity { get; set; }
        public DateTime TimeStamp { get; set; }
        public Type TypeOfElement { get; set; }

        #endregion Properties
    }
}