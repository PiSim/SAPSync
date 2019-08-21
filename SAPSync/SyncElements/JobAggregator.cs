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
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements
{
    public class JobAggregator : SyncElementBase, ISyncElement
    {
        #region Constructors

        public JobAggregator(string name = "", SyncElementConfiguration configuration = null)
        {
            
            Name = name;
            Configuration = configuration ?? new SyncElementConfiguration();
            ElementStatus = JobStatus.Idle;
            ReadElementData();
        }


        #endregion Constructors

        public void SetJobController(IJobController jobController)
        {
            if (JobController != null)
                UnsubscribeFromjobController(JobController);

            JobController = jobController;
            SubscribeToJobController(JobController);
        }

        protected virtual void SubscribeToJobController(IJobController jobController)
        {
            jobController.JobStarting += OnSyncTaskStarting;
            jobController.NewJobStarted += OnJobStarted;
        }

        protected virtual void UnsubscribeFromjobController(IJobController jobController)
        {
            jobController.JobStarting -= OnSyncTaskStarting;
            jobController.NewJobStarted -= OnJobStarted;
        }

        public IJobController JobController { get; protected set; }
        
        #region Events
        
        public event EventHandler SyncAborted;

        public event EventHandler SyncCompleted;
        
        public event EventHandler<SyncErrorEventArgs> SyncFailed;

        #endregion Events

        #region Properties

        public SyncElementConfiguration Configuration { get; private set; }
        public SyncElementData ElementData { get; protected set; }
        
        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();
        
        public IList<ISyncElement> RequiredElements { get; } = new List<ISyncElement>();

        public JobStatus ElementStatus { get; protected set; }

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods
                
        public virtual JobAggregator HasJob(ISyncOperation job)
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
                if (sender == CurrentTask)
                    SetOnQueue();
            }
        }

        protected virtual void SetOnQueue()
        {
            ChangeElementStatus(JobStatus.OnQueue);
        }

        public ICollection<ISyncOperation> Jobs { get; } = new List<ISyncOperation>();

        public override string Name { get; }

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

            if (ElementStatus != JobStatus.Failed && ElementStatus != JobStatus.Aborted)
                ChangeElementStatus(JobStatus.Completed);

            RaiseSyncCompleted();
        }

        protected virtual DateTime GetNextScheduledUpdate()
        {
            return ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);
        }

        protected override void Initialize()
        {
            base.Initialize();
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

        protected virtual void SaveElementData()
        {
            SSMDData.Execute(new UpdateEntityCommand<SSMDContext>(ElementData));
        }


        #endregion Methods

    }
}