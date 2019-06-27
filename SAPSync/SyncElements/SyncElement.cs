using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public abstract class SyncElement<T> : ISyncElement where T : class
    {
        #region Fields

        private bool _forbidUpdate, _enforceUpdate;

        #endregion Fields

        #region Constructors

        public SyncElement(SyncElementConfiguration configuration)
        {
            Configuration = configuration;
            SyncStatus = "";
            ReadElementData();
        }

        public SyncElement()
        {
            Configuration = new SyncElementConfiguration();
            SyncStatus = "";
            ReadElementData();
        }

        #endregion Constructors

        #region Events

        public event ProgressChangedEventHandler ProgressChanged;

        public event EventHandler<TaskEventArgs> ReadTaskCompleted;

        public event EventHandler<TaskEventArgs> ReadTaskStarting;

        public event EventHandler StatusChanged;

        public event EventHandler SyncAborted;

        public event EventHandler SyncCompleted;

        public event EventHandler SyncElementStarting;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        public event EventHandler SyncFailed;

        #endregion Events

        #region Properties

        public AbortToken AbortStatus { get; set; }
        public SyncElementConfiguration Configuration { get; private set; }
        public Task CurrentSyncTask { get; set; }

        public Task CurrentTask { get; set; }
        public SyncElementData ElementData { get; protected set; }

        public bool EnforceUpdate
        {
            get => _enforceUpdate;
            set
            {
                _enforceUpdate = value;
                if (value)
                    _forbidUpdate = !value;
            }
        }

        public bool ForbidUpdate
        {
            get => _forbidUpdate;
            set
            {
                _forbidUpdate = value;
                if (value)
                    _enforceUpdate = !value;
            }
        }

        public bool HasCompletedCurrentSyncTask { get; set; } = true;

        public bool IsFailed { get; set; } = false;

        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public bool MustPerformUpdate => !ForbidUpdate && (EnforceUpdate || IsUpForScheduledUpdate);

        public virtual string Name { get; }

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        public int PhaseProgress { get; set; }

        public IList<ISyncElement> RequiredElements { get; } = new List<ISyncElement>();

        public string SyncStatus { get; protected set; }

        protected bool IsSyncRunning { get; set; } = false;

        protected IRecordEvaluator<T> RecordEvaluator { get; set; }

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods

        public virtual void Clear()
        {
            RecordEvaluator = null;
        }

        public virtual SyncElement<T> DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                RequiredElements.Add(element);

            return this;
        }

        public async virtual void OnSyncTaskStarted(object sender, EventArgs e)
        {
            if (MustPerformUpdate)
            {
                await Task.WhenAll(RequiredElements
                    .Where(ele => ele.CurrentSyncTask != null)
                    .Select(req => req.CurrentSyncTask));

                CurrentSyncTask.Start();
            }
        }

        public virtual void OnSyncTaskStarting(object sender, EventArgs e)
        {
            ResetProgress();
            if (!HasCompletedCurrentSyncTask)
            {
                SetOnQueue();
                InitializeNewCurrentTask();
                RaiseSyncElementStarting();
            }
        }

        public void RaiseReadTaskStarting(Task t)
        {
            ReadTaskStarting?.Invoke(this, new TaskEventArgs(t));
        }

        public virtual void ResetProgress()
        {
            ChangeStatus("");
            RaisePhaseProgressChanged(0);
            HasCompletedCurrentSyncTask = !MustPerformUpdate;
        }

        public void SetOnQueue()
        {
            ChangeStatus("In Coda");
        }

        public virtual void StartSync()
        {
            IsSyncRunning = true;

            if (MustPerformUpdate)
            {
                try
                {
                    Initialize();
                    EnsureInitialized();
                }
                catch(Exception e)
                {
                    RaiseSyncError("Errore di inizializzazione: " + e.Message + "\t\tInnerException: " + e.InnerException?.Message);
                    SyncFailure();
                }
                if (AbortStatus == null)
                {
                    try
                    {
                        RunSyncronizationSequence();
                    }
                    catch (Exception e)
                    {
                        RaiseSyncError(e.Message);
                        SyncFailure();
                    }
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
            RaisePhaseProgressChanged(e.ProgressPercentage);
        }

        protected virtual void ChangeStatus(string newStatus)
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
            ChangeStatus("Cancellazione vecchi record");
            SSMDData.Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }

        protected virtual void EnsureInitialized()
        {
            if (RecordEvaluator == null || !RecordEvaluator.CheckInitialized())
                throw new InvalidOperationException("RecordEvaluator non inizializzato");
            if (ElementData == null)
                throw new InvalidOperationException("ElementData non inizializzato");
        }

        protected virtual void EvaluateRecords(IEnumerable<T> records)
        {
            if (records == null)
                throw new ArgumentNullException("RecordList");

            ChangeStatus("Controllo dei Record letti");
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

        protected virtual void FinalizeSync()
        {
            Clear();
            IsSyncRunning = false;
            HasCompletedCurrentSyncTask = true;
            ElementData.LastUpdate = DateTime.Now;

            RaisePhaseProgressChanged(100);
            RaiseSyncCompleted();

            try
            {
                SaveElementData();
            }
            catch (Exception e)
            {
                RaiseSyncError("Impossibile salvare ElementData: " + e.Message + "\t\tInnerException: " + e.InnerException?.Message);
            }
            
            if (AbortStatus != null)
                ChangeStatus("Annullato: " + AbortStatus.AbortReason);
            else if (IsFailed)
                ChangeStatus("Fallito");
            else
                ChangeStatus("Completato");
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
                RaiseReadTaskStarting(getResultsTask);
                getResultsTask.Start();
                getResultsTask.Wait();
                RaiseReadTaskCompleted(getResultsTask);
                records = getResultsTask.Result;
            }
            catch (Exception e)
            {
                RaiseSyncError("Errore di lettura: " + e.Message + "\t\tInnerException :" + e.InnerException.Message);
                throw new Exception("Lettura Record Fallita: " + e.Message, e);
            }
            
            try
            {
                var updatePackage = RecordEvaluator.GetUpdatePackage(records);
                UpdateDatabase(updatePackage);
            }
            catch (Exception e)
            {
                RaiseSyncError("Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException.Message);
            }
        }

        protected virtual void Initialize()
        {
            ChangeStatus("Inizializzazione");
            RaisePhaseProgressChanged(0);

            ConfigureRecordEvaluator();
            InitializeRecordEvaluator();
        }

        protected virtual void InitializeNewCurrentTask()
        {
            CurrentSyncTask = new Task(() => StartSync());
        }

        protected virtual void InitializeRecordEvaluator()
        {
            RecordEvaluator.Initialize(SSMDData);
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            ChangeStatus("Inserimento Nuovi record");
            RaisePhaseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            insertCommand.ProgressChanged += ChangeProgress;
            SSMDData.Execute(insertCommand);
        }

        protected void RaisePhaseProgressChanged(int newProgress)
        {
            PhaseProgress = newProgress;
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(newProgress, null);
            ProgressChanged?.Invoke(this, e);
        }

        protected virtual void RaiseReadTaskCompleted(Task t)
        {
            ReadTaskCompleted?.Invoke(this, new TaskEventArgs(t));
        }

        protected void RaiseStatusChanged()
        {
            EventArgs e = new EventArgs();
            StatusChanged?.Invoke(this, e);
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

        protected virtual void RaiseSyncElementStarting()
        {
            SyncElementStarting?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseSyncError(string errorMessage)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                ErrorMessage = errorMessage
            };

            SyncErrorRaised?.Invoke(this, args);
        }

        protected virtual void RaiseSyncFailed()
        {
            EventArgs args = new EventArgs();
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
                RaiseSyncError(e.Message);
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
                RaiseSyncError(e.Message);
                if (!Configuration.ContinueExportingOnImportFail)
                    throw new InvalidOperationException(errorMessage, e);
            }

            try
            {
                SyncStatus = "Esportazione Dati";
                RaiseStatusChanged();
                if (Configuration.PerformExport)
                    ExportData();
            }
            catch (Exception e)
            {
                string errorMessage = "Esportazione record fallita: " + e.Message;
                RaiseSyncError(e.Message);
                throw new InvalidOperationException(errorMessage, e);
            }
        }

        protected virtual void SaveElementData()
        {
            SSMDData.Execute(new UpdateEntityCommand<SSMDContext>(ElementData));
        }

        protected virtual void SyncFailure()
        {
            IsFailed = true;
            RaiseSyncFailed();
        }

        protected virtual void UpdateDatabase(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        protected virtual void UpdateExistingRecords(IEnumerable<T> records)
        {
            ChangeStatus("Aggiornamento record esistenti");
            RaisePhaseProgressChanged(0);
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