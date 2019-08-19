using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using SyncService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public abstract class SyncElement<T> : SyncElementBase, ISyncElement where T : class
    {
        #region Constructors

        public SyncElement(SyncElementConfiguration configuration)
        {
            Configuration = configuration;
            ElementStatus = SyncElementStatus.Idle;
            SyncStatus = SyncProgress.Idle;
            ReadElementData();
        }

        public SyncElement()
        {
            Configuration = new SyncElementConfiguration();
            ElementStatus = SyncElementStatus.Idle;
            SyncStatus = SyncProgress.Idle;
            ReadElementData();
        }

        public void SetCurrentTask(ISyncTask syncTask)
        {
            CurrentTask = syncTask;
        }
        
        #endregion Constructors

        public ISyncTask CurrentTask { get; protected set; }

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

        public override string Name => "SyncElement";

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        public int PhaseProgress { get; set; }

        public IList<ISyncElement> RequiredElements { get; } = new List<ISyncElement>();

        public SyncProgress SyncStatus { get; protected set; }

        public SyncElementStatus ElementStatus { get; protected set; }

        protected bool IsSyncRunning { get; set; } = false;

        protected IRecordEvaluator<T> RecordEvaluator { get; set; }

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods

        public void SetTaskController(ISyncTaskController taskController)
        {
            taskController.SyncTaskStarting += OnSyncTaskStarting;
            taskController.NewSyncTaskStarted += OnSyncTaskStarted;
        }

        public ISyncTaskController TaskController { get; protected set; }

        protected override void Clear()
        {
            base.Clear();
            RecordEvaluator = null;
            CurrentTask = null;
        }
        
        public virtual SyncElement<T> DependsOn(IEnumerable<ISyncElement> parentElements)
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

        public void SetOnQueue()
        {
            ChangeElementStatus(SyncElementStatus.OnQueue);
        }
        
        protected virtual void OnJobErrorRaised(object sender, SyncErrorEventArgs e)
        {
            RaiseSyncError(e.Exception, e.ErrorMessage, e.Severity);
        }

        protected virtual void OnJobStatusChanged(object sender, EventArgs e)
        {
            RaiseStatusChanged();
        }

        protected virtual void OnJobProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RaiseProgressChanged(e.ProgressPercentage);
        }


        public virtual void StartSync()
        {
            ElementStatus = SyncElementStatus.Running;
            
            try
            {
                Initialize();
                EnsureInitialized();
            }
            catch(Exception e)
            {
                SyncFailure(e);
            }
            
            if (AbortStatus == null)
            {
                try
                {
                    RunSyncronizationSequence();
                }
                catch (Exception e)
                {
                    SyncFailure(e);
                }
            }

            FinalizeSync();
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

        protected virtual void ConfigureRecordEvaluator()
        {
            RecordEvaluator = GetRecordEvaluator();
            RecordEvaluator.CheckRemovedRecords = Configuration.CheckDeletedElements;
            RecordEvaluator.IgnoreExistingRecords = Configuration.IgnoreExistingRecords;
        }

        protected virtual void DeleteRecords(IEnumerable<T> records)
        {
            ChangeSyncStatus(SyncProgress.ImportDelete);
            SSMDData.Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();

            if (RecordEvaluator == null || !RecordEvaluator.CheckInitialized())
                throw new InvalidOperationException("RecordEvaluator non inizializzato");
            if (ElementData == null)
                throw new InvalidOperationException("ElementData non inizializzato");
        }

        protected virtual void EvaluateRecords(IEnumerable<T> records)
        {
            if (records == null)
                throw new ArgumentNullException("RecordList");
        }

        protected abstract void ExecuteExport(IEnumerable<T> records);

        protected virtual void ExecutePostImportActions()
        {
        }

        protected virtual void ExportData()
        {
            ICollection<T> exportRecords = GetRecordsToExport();
            ExecuteExport(exportRecords);
        }

        protected virtual ICollection<ISyncJob> GetJobStack() => new List<ISyncJob>();

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

        protected virtual IQueryable<T> GetExportingRecordsQuery() => SSMDData.RunQuery(new Query<T, SSMDContext>());

        protected virtual DateTime GetNextScheduledUpdate()
        {
            return ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);
        }

        protected abstract IRecordEvaluator<T> GetRecordEvaluator();

        protected virtual ICollection<T> GetRecordsToExport() => GetExportingRecordsQuery().ToList();

        protected virtual void ImportData()
        {
            IEnumerable<T> records;
            try
            {
                Task<IEnumerable<T>> getResultsTask = new Task<IEnumerable<T>>(() => ReadRecords());
                RaiseExternalTaskStarting(getResultsTask);
                getResultsTask.Start();
                getResultsTask.Wait();
                RaiseExternalTaskCompleted(getResultsTask);
                records = getResultsTask.Result;
            }
            catch (Exception e)
            {
                RaiseSyncError(e: e,
                    errorMessage: "Errore di lettura: " + e.Message + "\t\tInnerException :" + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                throw new Exception("Lettura Record Fallita: " + e.Message, e);
            }
            
            try
            {
                var updatePackage = RecordEvaluator.GetUpdatePackage(records);
                UpdateDatabase(updatePackage);
            }
            catch (Exception e)
            {
                RaiseSyncError(e: e,
                    errorMessage: "Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            ChangeSyncStatus(SyncProgress.Initializing);
            RaiseProgressChanged(0);

            ConfigureRecordEvaluator();
            InitializeRecordEvaluator();
        }

        protected virtual void InitializeRecordEvaluator()
        {
            RecordEvaluator.Initialize(SSMDData);
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            ChangeSyncStatus(SyncProgress.ImportInsert);
            RaiseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            insertCommand.ProgressChanged += ChangeProgress;
            SSMDData.Execute(insertCommand);
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
            ElementData = SSMDData.RunQuery(new Query<SyncElementData, SSMDContext>()).SingleOrDefault(sed => sed.ElementType == this.GetType().ToString());
            if (ElementData == null)
                ElementData = new SyncElementData()
                {
                    LastUpdate = new DateTime(0),
                    ElementType = this.GetType().ToString()
                };
        }

        protected abstract IEnumerable<T> ReadRecords();

        protected virtual void RunSyncronizationSequence()
        {
            try
            {
                if (Configuration.PerformImport)
                    ImportData();
            }
            catch (Exception e)
            {
                string errorMessage = "Importazione record fallita: " + e.Message;
                RaiseSyncError(e:e,
                    errorMessage: e.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                if (!Configuration.ContinueExportingOnImportFail)
                    throw new InvalidOperationException(errorMessage, e);
            }

            try
            {
                if (Configuration.PerformImport)
                    ExecutePostImportActions();
            }
            catch (Exception e)
            {
                string errorMessage = "Errore nell'elaborazione post-importazione: " + e.Message;
                RaiseSyncError(e:e,
                    errorMessage: e.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                if (!Configuration.ContinueExportingOnImportFail)
                    throw new InvalidOperationException(errorMessage, e);
            }

            try
            {
                ChangeSyncStatus(SyncProgress.Export);
                RaiseStatusChanged();
                if (Configuration.PerformExport)
                    ExportData();
            }
            catch (Exception e)
            {
                string errorMessage = "Esportazione record fallita: " + e.Message;
                RaiseSyncError(e:e,
                    errorMessage: e.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                throw new InvalidOperationException(errorMessage, e);
            }
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

        protected virtual void UpdateDatabase(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        protected virtual void UpdateExistingRecords(IEnumerable<T> records)
        {
            ChangeSyncStatus(SyncProgress.ImportUpdate);
            RaiseProgressChanged(0);
            BatchUpdateEntitiesCommand<SSMDContext> updateCommand = new BatchUpdateEntitiesCommand<SSMDContext>(records);
            updateCommand.ProgressChanged += ChangeProgress;
            SSMDData.Execute(updateCommand);
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

    public class SyncElementConfiguration
    {
        #region Properties

        public bool CheckDeletedElements { get; set; } = false;
        public bool ContinueExportingOnImportFail { get; set; } = false;
        public bool IgnoreExistingRecords { get; set; } = false;
        public bool PerformExport { get; set; } = false;
        public bool PerformImport { get; set; } = false;

        #endregion Properties
    }
}