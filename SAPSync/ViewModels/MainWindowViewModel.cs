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
            _syncManager = new SyncManager();
            StartSyncCommand = new DelegateCommand(() => StartSync());
            ToggleSyncServiceCommand = new DelegateCommand(() => ToggleSyncService());
            OpenLogWindowCommand = new DelegateCommand(() => OpenLogWindow());
        }

        #endregion Constructors

        #region Events

        public event EventHandler SyncServiceStartRequested;

        public event EventHandler SyncServiceStopRequested;

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

        public SyncService.SyncService SyncService { get; set; }
        public DelegateCommand ToggleSyncServiceCommand { get; set; }

        #endregion Properties

        #region Methods

        public List<SyncElementViewModel> GetSyncElements(SyncManager syncManager) => syncManager.SyncElements.Select(sel => new SyncElementViewModel(sel)).ToList();

        protected virtual void OnServiceToggle(object sender, EventArgs e)
        {
            RaisePropertyChanged("ServiceStatus");
        }

        private async void StartSync()
        {
            await Task.Run(() => _syncManager?.StartSync());
        }

        private void ToggleSyncService()
        {
            if (ServiceStatus == "Running")
                SyncServiceStopRequested?.Invoke(this, new EventArgs());
            else if (ServiceStatus == "Stopped")
                SyncServiceStartRequested?.Invoke(this, new EventArgs());
        }

        #endregion Methods
    }
}