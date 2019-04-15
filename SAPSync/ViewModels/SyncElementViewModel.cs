using Prism.Mvvm;
using SAPSync.SyncElements;
using System;
using System.ComponentModel;

namespace SAPSync.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {
        #region Constructors

        public SyncElementViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public bool IsSelected { get => SyncElement.RequiresSync; set => SyncElement.RequiresSync = value; }
        public string Name => SyncElement.Name;
        public int PhaseProgress => SyncElement.PhaseProgress;
        public string Status => SyncElement.SyncStatus;
        public ISyncElement SyncElement { get; set; }

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

        #endregion Methods
    }
}