using Prism.Mvvm;
using SAPSync.SyncElements;
using System;
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

        public bool IsSelected { get => SyncElement.EnforceUpdate; set => SyncElement.EnforceUpdate = value; }
        public bool IsUpdateForbidden { get => SyncElement.ForbidUpdate; set => SyncElement.ForbidUpdate = value; }
        public DateTime? LastUpdate => SyncElement.LastUpdate;
        public string Name => SyncElement.Name;
        public DateTime? NextScheduledUpdate => SyncElement.NextScheduledUpdate;
        public int PhaseProgress => SyncElement.PhaseProgress;
        public string Status => SyncElement.SyncStatus;
        public ISyncElement SyncElement => _syncElement;

        #endregion Properties

        #region Methods

        public void OnPhaseProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RaisePropertyChanged("PhaseProgress");
        }

        public void OnStatusChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("Status");
        }

        public void OnSyncCompleted(object sender, EventArgs e)
        {
            RaisePropertyChanged("LastUpdate");
            RaisePropertyChanged("NextScheduledUpdate");
        }

        #endregion Methods
    }
}