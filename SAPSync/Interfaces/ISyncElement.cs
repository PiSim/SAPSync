using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SAPSync
{

    public enum SyncElementStatus
    {
        Idle,
        OnQueue,
        Running,
        Aborted,
        Failed,
        Completed,
        Stopped
    }

    public enum SyncProgress
    {
        Idle,
        Initializing,
        ImportRead,
        ImportInsert,
        ImportUpdate,
        ImportDelete,
        PostImportActions,
        Export,
        Finalizing
    }

    public interface ISyncElement : ISyncBase
    {

        #region Events

        event EventHandler SyncCompleted;

        event EventHandler<SyncErrorEventArgs> SyncFailed;

        #endregion Events

        #region Properties

        ISyncTask CurrentTask { get; }
        ISyncTaskController TaskController { get; }
        void SetCurrentTask( ISyncTask syncTask);
        bool HasPendingRequirements { get; }
        bool IsFailed { get; set; }
        bool IsUpForScheduledUpdate { get; }
        DateTime? LastUpdate { get; }
        DateTime? NextScheduledUpdate { get; }
        int PhaseProgress { get; }
        SyncProgress SyncStatus { get; }
        SyncElementStatus ElementStatus { get; }
        void SetTaskController(ISyncTaskController controller);

        #endregion Properties

        #region Methods

        void StartSync();

        #endregion Methods
    }

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