using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SAPSync.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private string _serviceStatus;
        private List<SyncElementViewModel> _syncElements;
        private SyncManager _syncManager;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            StartSyncCommand = new DelegateCommand(() => StartSync());
            ToggleSAPSyncCommand = new DelegateCommand(() => ToggleSAPSync());
            OpenLogWindowCommand = new DelegateCommand(() => OpenLogWindow());
        }

        #endregion Constructors

        #region Events

        public event EventHandler SAPSyncStartRequested;

        public event EventHandler SAPSyncStopRequested;

        #endregion Events

        #region Properties

        public string ServiceStatus
        { get => _serviceStatus; set { _serviceStatus = value; RaisePropertyChanged("ServiceStatus"); } }

        public DelegateCommand StartSyncCommand { get; set; }

        private void OpenLogWindow()
        {
            Window logWindow = new Views.LogDialog();
            logWindow.Show();
        }

        public DelegateCommand OpenLogWindowCommand { get; }

        public List<SyncElementViewModel> SyncElements
        {
            get => _syncElements;
            set
            {
                _syncElements = value;
                RaisePropertyChanged("SyncElements");
            }
        }

        public SyncManager SyncManager
        {
            get => _syncManager;
            set
            {
                _syncManager = value;
                SyncElements = GetSyncElements(_syncManager);
            }
        }

        public SyncService SAPSync { get; set; }
        public DelegateCommand ToggleSAPSyncCommand { get; set; }

        #endregion Properties

        #region Methods

        public List<SyncElementViewModel> GetSyncElements(SyncManager syncManager) => syncManager.SyncElements.Select(sel => new SyncElementViewModel(sel)).ToList();

        protected virtual void OnServiceToggle(object sender, EventArgs e)
        {
            RaisePropertyChanged("ServiceStatus");
        }

        private void StartSync()
        {
            Task.Run(() => _syncManager?.StartSync(SyncElements.Where(sel => sel.IsSelected).Select(sel => sel.SyncElement)));
        }

        private void ToggleSAPSync()
        {
            if (ServiceStatus == "Running")
                SAPSyncStopRequested?.Invoke(this, new EventArgs());
            else if (ServiceStatus == "Stopped")
                SAPSyncStartRequested?.Invoke(this, new EventArgs());
        }

        #endregion Methods
    }
}