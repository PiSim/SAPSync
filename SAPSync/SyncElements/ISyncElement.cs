using System;
using System.ComponentModel;

namespace SAPSync.SyncElements
{
    public interface ISyncElement
    {
        #region Events

        event ProgressChangedEventHandler ProgressChanged;

        event EventHandler StatusChanged;

        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        event EventHandler SyncFailed;

        #endregion Events

        #region Properties

        DateTime LastUpdate { get; }
        string Name { get; }
        DateTime NextScheduledUpdate { get; }
        int PhaseProgress { get; }

        bool RequiresSync { get; set; }

        string SyncStatus { get; }

        #endregion Properties

        #region Methods

        void ResetProgress();

        void SetOnQueue();

        void StartSync();

        #endregion Methods
    }

    public class SyncErrorEventArgs : EventArgs
    {
        #region Properties

        public string ErrorMessage { get; set; }

        #endregion Properties
    }
}