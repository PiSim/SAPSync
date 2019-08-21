using Prism.Mvvm;
using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAPSync.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {

        #region Fields

        #endregion Fields

        #region Constructors

        public SyncElementViewModel(ISyncElement syncElement)
        {
            SyncElement = syncElement;
            SyncElement.ProgressChanged += OnPhaseProgressChanged;
            SyncElement.StatusChanged += OnStatusChanged;
            SyncElement.SyncCompleted += OnSyncCompleted;
        }

        #endregion Constructors

        #region Properties

        private readonly Dictionary<SyncProgress, string> _progressIndex = new Dictionary<SyncProgress, string>()
        {
            { SyncProgress.Export, "" },
            {SyncProgress.Idle, "" },
            {SyncProgress.ImportRead, "Lettura record" },
            {SyncProgress.ImportDelete, "Cancellazione record" },
            {SyncProgress.ImportInsert, "Inserimento record" },
            {SyncProgress.ImportUpdate, "Aggiornamento record" }
        };
        private readonly Dictionary<JobStatus, string> _statusIndex = new Dictionary<JobStatus, string>()
        {
            {JobStatus.Idle, "" },
            {JobStatus.OnQueue, "In Coda" },
            {JobStatus.Running, "In Esecuzione" },
            {JobStatus.Stopped, "Interrotto" },
            {JobStatus.Completed, "Completato" },
            {JobStatus.Failed, "Fallito" }
        };
        public bool IsSelected { get; set; }
        public bool IsUpdateForbidden { get; set; }
        public DateTime? LastUpdate => SyncElement.LastUpdate;
        public string Name => SyncElement.Name;
        public DateTime? NextScheduledUpdate => SyncElement.NextScheduledUpdate;
        public int PhaseProgress { get; set; } = 0;

        public string SyncStatus { get; set; }

        public string ElementStatus
        {
            get
            {
                if (_statusIndex.ContainsKey(SyncElement.ElementStatus))
                    return _statusIndex[SyncElement.ElementStatus];

                else
                    return "";
            }
        }
        public ISyncElement SyncElement { get; }

        #endregion Properties

        #region Methods

        public void OnPhaseProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PhaseProgress = e.ProgressPercentage;
            RaisePropertyChanged("PhaseProgress");
            if (sender is ISyncBase)
            {
                SyncStatus = (sender as ISyncBase).Name;
                RaisePropertyChanged("SyncStatus");
            }
        }

        public void OnStatusChanged(object sender, EventArgs e)
        {
            if (sender is ISyncElement)
                RaisePropertyChanged("ElementStatus");
        }

        public void OnSyncCompleted(object sender, EventArgs e)
        {
            RaisePropertyChanged("LastUpdate");
            RaisePropertyChanged("NextScheduledUpdate");
        }

        #endregion Methods
    }
}