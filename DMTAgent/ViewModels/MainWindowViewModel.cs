using DMTAgent.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DMTAgent.LogListener;

namespace DMTAgent.ViewModels
{
    public class MainWindowViewModel : BindableBase, IViewModel<Views.MainWindow>
    {
        #region Fields

        private LinkedList<string> _log;
        private bool _showCompleteJobs;
        private ISyncAgent _syncAgent;
        private List<SyncElementViewModel> _syncElements;
        private ISyncManager _syncManager;

        #endregion Fields

        #region Constructors

        // SP Added dependency on ISyncAgent, removed start/stoprequested events in favor of direct calls to the interface methods
        public MainWindowViewModel(ISyncManager syncManager,
            LogListener listener,
            ISyncAgent syncAgent)
        {
            _syncAgent = syncAgent;
            _syncManager = syncManager;

            _log = new LinkedList<string>();
            listener.LogCreated += OnLogCreated;

            _showCompleteJobs = true;
            StartSyncCommand = new RelayCommand(() => StartSync());
            ToggleDMTAgentCommand = new RelayCommand(() => ToggleDMTAgent());

            SyncElements = GetSyncElements(_syncManager);

            _syncAgent.StatusChanged += OnAgentStatusChanged;
            _syncManager.JobController.NewJobStarted += OnJobStarted;
            _syncManager.JobController.JobCompleted += OnJobCompleted;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<SubJobViewModel> ActiveJobs
            => _syncManager?
            .JobController?
            .GetJobs(ShowCompleteJobs)?
            .OrderByDescending(job => job.StartTime)
            .SelectMany(job => job.SubJobs)
            .Select(sjb => new SubJobViewModel(sjb));

        public string AgentStatus => _syncAgent.Status.ToString();
        public string Log => string.Concat(_log.ToList().Select(line => line + '\n'));
        public int MaxLogLines { get; } = 1000;
        public RelayCommand OpenLogWindowCommand { get; }

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

        public RelayCommand ToggleDMTAgentCommand { get; set; }

        #endregion Properties

        #region Methods

        public List<SyncElementViewModel> GetSyncElements(ISyncManager syncManager) => syncManager.SyncElements.Select(sel => new SyncElementViewModel(sel)).ToList();

        protected virtual void OnAgentStatusChanged(object sender, EventArgs e) => RaisePropertyChanged("AgentStatus");

        protected virtual void OnJobCompleted(object sender, EventArgs e)
        {
            RaisePropertyChanged("ActiveJobs");
            foreach (SyncElementViewModel el in SyncElements)
                el.RaiseChange();
        }

        protected virtual void OnJobStarted(object sender, EventArgs e) => RaisePropertyChanged("ActiveJobs");

        protected virtual void OnLogCreated(object sender, LogCreatedEventArgs e)
        {
            if (_log.Count > MaxLogLines)
                _log.RemoveLast();

            _log.AddFirst(e.LogEventInfo.FormattedMessage);
            RaisePropertyChanged("Log");
        }

        private void OnLogEntryCreated(object sender, EventArgs e) => RaisePropertyChanged("CurrentLog");

        private void StartSync()
        {
            Task.Run(() => _syncManager?.StartSync(SyncElements.Where(sel => sel.IsSelected).Select(sel => sel.SyncElement)));
        }

        private void ToggleDMTAgent()
        {
            if (AgentStatus == "Running")
                _syncAgent.Stop();
            else if (AgentStatus == "Stopped")
                _syncAgent.Start();
        }

        #endregion Methods
    }
}