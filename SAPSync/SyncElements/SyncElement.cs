using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SAPSync.SyncElements
{
    public abstract class SyncElement<T> : ISyncElement where T : class
    {
        #region Fields

        protected SSMDData _sSMDData;

        #endregion Fields

        #region Constructors

        public SyncElement(SSMDData sSMDData)
        {
            SyncStatus = "";
            _sSMDData = sSMDData;
        }

        #endregion Constructors

        #region Events

        public event ProgressChangedEventHandler ProgressChanged;

        public event EventHandler StatusChanged;

        public event EventHandler SyncAborted;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        public event EventHandler SyncFailed;

        #endregion Events

        #region Properties

        public AbortToken AbortStatus { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Name { get; protected set; }
        public DateTime NextScheduledUpdate { get; set; }
        public int PhaseProgress { get; set; }
        public bool RequiresSync { get; set; } = false;
        public string SyncStatus { get; protected set; }
        protected bool IsFailed { get; set; } = false;
        protected IRecordEvaluator<T> RecordEvaluator { get; set; }
        private bool ContinueExportingOnImportFail { get; set; }

        #endregion Properties

        #region Methods

        public virtual void Clear()
        {
            RecordEvaluator = null;
        }

        public virtual void ResetProgress()
        {
            ChangeStatus("");
            RaisePhaseProgressChanged(0);
        }

        public void SetOnQueue()
        {
            ChangeStatus("In Coda");
        }

        public virtual void StartSync()
        {
            Initialize();
            EnsureInitialized();
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

        protected abstract void ConfigureRecordEvaluator();

        protected virtual void DeleteRecords(IEnumerable<T> records)
        {
            ChangeStatus("Cancellazione vecchi record");
            _sSMDData.Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }

        protected virtual void EnsureInitialized()
        {
            if (RecordEvaluator == null || !RecordEvaluator.CheckInitialized())
                throw new InvalidOperationException("RecordEvaluator non inizializzato");

            if (_sSMDData == null)
                throw new ArgumentNullException("SSMDData");
        }

        protected virtual void EvaluateRecords(IEnumerable<T> records)
        {
            if (records == null)
                throw new ArgumentNullException("RecordList");

            ChangeStatus("Controllo dei Record letti");
        }

        protected virtual void ExecutePostImportActions()
        {
        }

        protected virtual void ExportData()
        {
            ICollection<T> exportRecords = GetRecordsToExport();
            ExecuteExport(exportRecords);
        }

        protected virtual void ExecuteExport(IEnumerable<T> records)
        {

        }
            

        protected virtual ICollection<T> GetRecordsToExport() => GetExportingRecordsQuery().ToList();

        protected virtual IQueryable<T> GetExportingRecordsQuery() => _sSMDData.RunQuery(new Query<T, SSMDContext>());
        
        protected virtual void FinalizeSync()
        {
            RaisePhaseProgressChanged(100);

            ChangeStatus("Pulizia della memoria");
            Clear();

            if (AbortStatus != null)
                ChangeStatus("Annullato: " + AbortStatus.AbortReason);
            else if (IsFailed)
                ChangeStatus("Fallito");
            else
                ChangeStatus("Completato");
        }

        protected virtual void ImportData()
        {
            var records = ReadRecords();
            var updatePackage = RecordEvaluator.GetUpdatePackage(records);
            UpdateDatabase(updatePackage);
        }

        protected virtual void Initialize()
        {
            ChangeStatus("Inizializzazione");
            RaisePhaseProgressChanged(0);

            ConfigureRecordEvaluator();
            InitializeRecordEvaluator();
        }

        protected virtual void InitializeRecordEvaluator()
        {
            RecordEvaluator.Initialize(_sSMDData);
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            ChangeStatus("Inserimento Nuovi record");
            RaisePhaseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            insertCommand.ProgressChanged += ChangeProgress;
            _sSMDData.Execute(insertCommand);
        }

        protected void RaisePhaseProgressChanged(int newProgress)
        {
            PhaseProgress = newProgress;
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(newProgress, null);
            ProgressChanged?.Invoke(this, e);
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

        protected abstract IEnumerable<T> ReadRecords();

        protected virtual void RunSyncronizationSequence()
        {
            try
            {
                if (PerformImport)
                    ImportData();
            }
            catch (Exception e)
            {
                string errorMessage = "Importazione record fallita: " + e.Message;
                RaiseSyncError(e.Message);
                if (!ContinueExportingOnImportFail)
                    throw new InvalidOperationException(errorMessage, e);
            }

            try
            {

                ExecutePostImportActions();
            }
            catch (Exception e)
            {
                string errorMessage = "Errore nell'elaborazione post-importazione: " + e.Message;
                RaiseSyncError(e.Message);
                if (!ContinueExportingOnImportFail)
                    throw new InvalidOperationException(errorMessage, e);
            }

            try
            {
                if (PerformExport)
                    ExportData();
            }
            catch (Exception e)
            {
                string errorMessage = "Esportazione record fallita: " + e.Message;
                RaiseSyncError(e.Message);
                throw new InvalidOperationException(errorMessage, e);
            }
        }

        public bool PerformImport { get; set; } = true;
        public bool PerformExport { get; set; } = false;


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
            _sSMDData.Execute(updateCommand);
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