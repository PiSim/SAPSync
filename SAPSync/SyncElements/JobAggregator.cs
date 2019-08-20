using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public class JobAggregator : SyncElementBase, ISyncElement
    {
        #region Constructors

        public JobAggregator(string name = "", SyncElementConfiguration configuration = null)
        {
            
            Name = name;
            Configuration = configuration ?? new SyncElementConfiguration();
            ElementStatus = SyncElementStatus.Idle;
            SyncStatus = SyncProgress.Idle;
            ReadElementData();
        }


        #endregion Constructors

        public void SetTaskController(ISyncTaskController taskController)
        {
            if (TaskController != null)
                UnsubscribeFromTaskController(TaskController);

            TaskController = taskController;
            SubscribeToTaskController(TaskController);
        }

        protected virtual void SubscribeToTaskController(ISyncTaskController taskController)
        {
            taskController.SyncTaskStarting += OnSyncTaskStarting;
            taskController.NewSyncTaskStarted += OnSyncTaskStarted;
        }

        protected virtual void UnsubscribeFromTaskController(ISyncTaskController taskController)
        {
            taskController.SyncTaskStarting -= OnSyncTaskStarting;
            taskController.NewSyncTaskStarted -= OnSyncTaskStarted;
        }

        public ISyncTaskController TaskController { get; protected set; }
        
        #region Events
        
        public event EventHandler SyncAborted;

        public event EventHandler SyncCompleted;
        
        public event EventHandler<SyncErrorEventArgs> SyncFailed;

        #endregion Events

        #region Properties

        private SyncElementStatus[] PendingStates => new SyncElementStatus[] { SyncElementStatus.Running, SyncElementStatus.OnQueue };

        public AbortToken AbortStatus { get; set; }
        public SyncElementConfiguration Configuration { get; private set; }

        public SyncElementData ElementData { get; protected set; }

        public bool HasPendingRequirements => RequiredElements.Count != 0
            && RequiredElements.Any(rel => PendingStates.Contains(rel.ElementStatus)); 

        public bool IsFailed { get; set; } = false;

        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        public int PhaseProgress { get; set; }

        public IList<ISyncElement> RequiredElements { get; } = new List<ISyncElement>();

        public SyncProgress SyncStatus { get; protected set; }

        public SyncElementStatus ElementStatus { get; protected set; }

        protected bool IsSyncRunning { get; set; } = false;

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods
                
        public virtual JobAggregator HasJob(ISyncJob job)
        {
            Jobs.Add(job);
            SubscribeToElement(job);
            return this;
        }

        public virtual ISyncElement DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                RequiredElements.Add(element);

            return this;
        }

        public virtual bool IsActive => PendingStates.Contains(ElementStatus);

        public virtual void OnSyncTaskStarting(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                ResetProgress();
                if (sender == CurrentTask)
                    SetOnQueue();
            }
        }

        public virtual void ResetProgress()
        {
            IsFailed = false;
            ChangeSyncStatus(SyncProgress.Idle);
            RaiseProgressChanged(0);
        }
           
        protected virtual void SetOnQueue()
        {
            ChangeElementStatus(SyncElementStatus.OnQueue);
        }

        public ICollection<ISyncJob> Jobs { get; } = new List<ISyncJob>();

        public override string Name { get; }

        public ISyncTask CurrentTask { get; protected set; }

        protected virtual void ExecuteJobStack()
        {
            foreach (ISyncJob job in Jobs)
            {
                SubscribeToElement(job);
                job.Run();
                job.Dispose();
            }
        }

        public virtual void StartSync() => Run();


        protected override void Execute()
        {
            base.Execute();
            ElementStatus = SyncElementStatus.Running;
            ExecuteJobStack();
        }

        protected override void OnCompleting()
        {
            FinalizeSync();
            base.OnCompleting();
        }

        protected void Abort(string abortReason)
        {
            AbortStatus = new AbortToken() { AbortReason = abortReason };
            RaiseSyncAborted();
        }

        protected void ChangeProgress(object sender, ProgressChangedEventArgs e)
        {
            RaiseProgressChanged(e.ProgressPercentage);
        }

        protected virtual void ChangeSyncStatus(SyncProgress newStatus)
        {
            SyncStatus = newStatus;
            RaiseStatusChanged();
        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (ElementData == null)
                throw new InvalidOperationException("ElementData non inizializzato");
        }

        protected override void Clear()
        {
            base.Clear();
            CurrentTask = null;
        }

        protected virtual void FinalizeSync()
        {
            Clear();
            IsSyncRunning = false;
            ElementData.LastUpdate = DateTime.Now;

            RaiseProgressChanged(100);

            try
            {
                SaveElementData();
            }
            catch (Exception e)
            {
                RaiseSyncError(errorMessage: "Impossibile salvare ElementData: " + e.Message + "\t\tInnerException: " + e.InnerException?.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Minor);
            }

            if (ElementStatus != SyncElementStatus.Failed && ElementStatus != SyncElementStatus.Aborted)
                ChangeElementStatus(SyncElementStatus.Completed);

            RaiseSyncCompleted();
        }

        protected virtual DateTime GetNextScheduledUpdate()
        {
            return ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);
        }

        protected override void Initialize()
        {
            base.Initialize();
            ChangeSyncStatus(SyncProgress.Initializing);
            RaiseProgressChanged(0);
        }


        protected virtual void RaiseSyncAborted()
        {
            SyncAborted?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseSyncCompleted()
        {
            EventArgs e = new EventArgs();
            SyncCompleted?.Invoke(this, e);
        }

        protected virtual void RaiseSyncFailed(Exception e = null)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                ErrorMessage = "Sincronizzazione fallita",
                NameOfElement = Name,
                Progress = SyncStatus,
                Severity = SyncErrorEventArgs.ErrorSeverity.Critical,
                TypeOfElement = GetType()
            };

            SyncFailed?.Invoke(this, args);
        }

        protected virtual void ReadElementData()
        {
            ElementData = SSMDData.RunQuery(new Query<SyncElementData, SSMDContext>()).FirstOrDefault(sed => sed.ElementType == this.Name);
            if (ElementData == null)
                ElementData = new SyncElementData()
                {
                    LastUpdate = new DateTime(0),
                    ElementType = Name
                };
        }

        public void OnSyncTaskStarted(object sender, EventArgs e)
        {

        }
        
        protected virtual void SaveElementData()
        {
            SSMDData.Execute(new UpdateEntityCommand<SSMDContext>(ElementData));
        }

        protected virtual void ChangeElementStatus(SyncElementStatus newStatus)
        {
            ElementStatus = newStatus;
            RaiseStatusChanged();
        }

        protected virtual void SyncFailure(Exception e = null)
        {
            RaiseSyncFailed(e);
            ChangeElementStatus(SyncElementStatus.Failed);
            ChangeSyncStatus(SyncProgress.Idle);
        }

        public void SetCurrentTask(ISyncTask syncTask)
        {
            CurrentTask = syncTask;
        }

        #endregion Methods

        #region Classes

        public class AbortToken
        {
            #region Properties

            public string AbortReason { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}