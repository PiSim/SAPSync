using Prism.Mvvm;
using SAPSync.SyncElements;
using SyncService;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAPSync.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {
        #region Fields

        private readonly ISyncElement _syncElement;

        #endregion Fields

        #region Constructors

        public SyncElementViewModel(ISyncElement syncElement)
        {
            _syncElement = syncElement;
            _syncElement.ProgressChanged += OnPhaseProgressChanged;
            _syncElement.StatusChanged += OnStatusChanged;
            _syncElement.SyncCompleted += OnSyncCompleted;
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
        private readonly Dictionary<SyncElementStatus, string> _statusIndex = new Dictionary<SyncElementStatus, string>()
        {
            {SyncElementStatus.Idle, "" },
            {SyncElementStatus.OnQueue, "In Coda" },
            {SyncElementStatus.Running, "In Esecuzione" },
            {SyncElementStatus.Stopped, "Interrotto" },
            {SyncElementStatus.Completed, "Completato" },
            {SyncElementStatus.Failed, "Fallito" }
        };
        public bool IsSelected { get; set; }
        public bool IsUpdateForbidden { get; set; }
        public DateTime? LastUpdate => SyncElement.LastUpdate;
        public string Name => SyncElement.Name;
        public DateTime? NextScheduledUpdate => SyncElement.NextScheduledUpdate;
        public int PhaseProgress => SyncElement.PhaseProgress;

        public string SyncStatus
        {
            get
            {
                if (_progressIndex.ContainsKey(SyncElement.SyncStatus))
                    return _progressIndex[SyncElement.SyncStatus];

                else
                    return "";
            }
        }

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
        public ISyncElement SyncElement => _syncElement;

        #endregion Properties

        #region Methods

        public void OnPhaseProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RaisePropertyChanged("PhaseProgress");
        }

        public void OnStatusChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("SyncStatus");
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