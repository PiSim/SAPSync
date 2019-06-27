using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public interface ISyncElement
    {
        #region Events

        event ProgressChangedEventHandler ProgressChanged;

        event EventHandler<TaskEventArgs> ReadTaskCompleted;

        event EventHandler<TaskEventArgs> ReadTaskStarting;

        event EventHandler StatusChanged;

        event EventHandler SyncCompleted;

        event EventHandler SyncElementStarting;

        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        event EventHandler SyncFailed;

        #endregion Events

        #region Properties

        Task CurrentSyncTask { get; }
        bool EnforceUpdate { get; set; }
        bool ForbidUpdate { get; set; }
        bool HasCompletedCurrentSyncTask { get; }
        bool IsFailed { get; set; }
        bool IsUpForScheduledUpdate { get; }
        DateTime? LastUpdate { get; }
        bool MustPerformUpdate { get; }
        string Name { get; }
        DateTime? NextScheduledUpdate { get; }
        int PhaseProgress { get; }
        string SyncStatus { get; }

        #endregion Properties

        #region Methods

        void OnSyncTaskStarted(object sender, EventArgs e);

        void OnSyncTaskStarting(object sender, EventArgs e);

        #endregion Methods
    }

    public class SyncErrorEventArgs : EventArgs
    {
        #region Properties

        public string ErrorMessage { get; set; }

        #endregion Properties
    }
}