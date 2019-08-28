using DMTAgent.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMTAgent.ViewModels
{
    public class MainWindowViewModel : BindableBase, IViewModel<Views.MainWindow>
    {
        #region Fields

        private string _serviceStatus;
        private List<SyncElementViewModel> _syncElements;
        private SyncManager _syncManager;
        private bool _showCompleteJobs;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            _showCompleteJobs = true;
            // SyncLogger.LogEntryCreated += OnLogEntryCreated;
            StartSyncCommand = new RelayCommand(() => StartSync());
            ToggleDMTAgentCommand = new RelayCommand(() => ToggleDMTAgent());
        }

        private void OnLogEntryCreated(object sender, EventArgs e)
        {
            RaisePropertyChanged("CurrentLog");
        }

        #endregion Constructors

        #region Events

        public event EventHandler DMTAgentStartRequested;

        public event EventHandler DMTAgentStopRequested;

        #endregion Events

        #region Properties

        public IEnumerable<SubJobViewModel> ActiveJobs
            => SyncManager?
            .JobController?
            .GetJobs(ShowCompleteJobs)?
            .OrderByDescending(job => job.StartTime)
            .SelectMany(job => job.SubJobs)
            .Select(sjb => new SubJobViewModel(sjb));

        // public string CurrentLog => SyncLogger.CurrentLog.Join(Environment.NewLine);

        public RelayCommand OpenLogWindowCommand { get; }

        public string ServiceStatus
        { get => _serviceStatus; set { _serviceStatus = value; RaisePropertyChanged("ServiceStatus"); } }

        public bool ShowCompleteJobs
        {
            get => _showCompleteJobs;
            set
            {
                _showCompleteJobs = value;
                RaisePropertyChanged("ActiveJobs");
            }
        }

        public RelayCommand StartSyncCommand { get; set; }

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

                if (_syncManager != null)
                {
                    _syncManager.JobController.NewJobStarted += OnJobStarted;
                    _syncManager.JobController.JobCompleted += OnJobCompleted;
                }
            }
        }

        public RelayCommand ToggleDMTAgentCommand { get; set; }

        #endregion Properties

        #region Methods

        public List<SyncElementViewModel> GetSyncElements(SyncManager syncManager) => syncManager.SyncElements.Select(sel => new SyncElementViewModel(sel)).ToList();

        protected virtual void OnJobCompleted(object sender, EventArgs e) => RaisePropertyChanged("ActiveJobs");

        protected virtual void OnJobStarted(object sender, EventArgs e) => RaisePropertyChanged("ActiveJobs");

        private void StartSync()
        {
            Task.Run(() => _syncManager?.StartSync(SyncElements.Where(sel => sel.IsSelected).Select(sel => sel.SyncElement)));
        }

        private void ToggleDMTAgent()
        {
            if (ServiceStatus == "Running")
                DMTAgentStopRequested?.Invoke(this, new EventArgs());
            else if (ServiceStatus == "Stopped")
                DMTAgentStartRequested?.Invoke(this, new EventArgs());
        }

        #endregion Methods
    }
}