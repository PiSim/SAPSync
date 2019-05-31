using DataAccessCore.Commands;
using SAP.Middleware.Connector;
using SAPSync.SyncElements;
using SSMD;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAPSync
{
    public abstract class SyncElement<T> : ISyncElement where T : class
    {
        #region Fields

        protected IList<T> _recordList;

        protected IList<T> _recordsToDelete;

        protected IList<T> _recordsToInsert;

        protected IList<T> _recordsToUpdate;

        protected RfcDestination _rfcDestination;

        protected SSMDData _sSMDData;

        #endregion Fields

        #region Constructors

        public SyncElement()
        {
            SyncStatus = "";
        }

        #endregion Constructors

        #region Events

        public event ProgressChangedEventHandler ProgressChanged;

        public event EventHandler StatusChanged;

        public event EventHandler SyncAborted;

        #endregion Events

        #region Properties

        public AbortToken AbortStatus { get; set; }

        public string Name { get; protected set; }

        public int PhaseProgress { get; set; }

        public bool RequiresSync { get; set; } = true;

        public string SyncStatus { get; protected set; }

        protected IRecordEvaluator<T> RecordEvaluator { get; set; }

        protected IRecordValidator<T> RecordValidator { get; set; }

        #endregion Properties

        #region Methods

        public void SetOnQueue()
        {
            RaiseStatusChanged("In Coda");
        }

        public virtual void Clear()
        {
            RecordEvaluator = null;
            RecordValidator = null;
            _recordsToDelete = null;
            _recordsToInsert = null;
            _recordsToUpdate = null;
            _recordList = null;
            _sSMDData = null;
            _rfcDestination = null;
        }

        public virtual void StartSync(RfcDestination rfcDestination, SSMDData sSMDData)
        {
            _sSMDData = sSMDData;
            _rfcDestination = rfcDestination;

            Initialize();
            EnsureInitialized();
            if (AbortStatus == null)
            {
                RetrieveSAPRecords();
                EvaluateRecords();

                InsertNewRecords();
                UpdateExistingRecords();
                OnUpdateCompleted();
                DeleteOldRecords();
            }

            FinalizeSync();
        }

        protected void Abort(string abortReason)
        {
            AbortStatus = new AbortToken() { AbortReason = abortReason };
            RaiseSyncAborted();
        }

        protected virtual void AddRecordToDeletes(T record)
        {
            _recordsToDelete.Add(record);
        }

        protected virtual void AddRecordToInserts(T record) => _recordsToInsert.Add(RecordValidator.GetInsertableRecord(record));

        protected virtual void AddRecordToUpdates(T record) => _recordsToUpdate.Add(RecordValidator.GetInsertableRecord(record));

        protected void ChangeProgress(object sender, ProgressChangedEventArgs e)
        {
            RaisePhaseProgressChanged(e.ProgressPercentage);
        }

        protected virtual void DeleteOldRecords()
        {
            RaiseStatusChanged("Cancellazione vecchi record");
            _sSMDData.Execute(new DeleteEntitiesCommand<SSMDContext>(_recordsToDelete));
        }

        protected virtual void EnsureInitialized()
        {
            if (_recordsToDelete == null
                || _recordsToInsert == null
                || _recordsToUpdate == null)
                throw new InvalidOperationException("Liste Record non inizializzate");

            if (RecordEvaluator == null)
                throw new InvalidOperationException("RecordEvaluator non inizializzato");

            if (!RecordEvaluator.CheckIndexInitialized())
                throw new InvalidOperationException("Indice Record non inizializzato");

            if (_rfcDestination == null)
                throw new ArgumentNullException("RfcDestination");

            if (_sSMDData == null)
                throw new ArgumentNullException("SSMDData");
        }

        public virtual void ResetProgress()
        {
            RaiseStatusChanged("");
            RaisePhaseProgressChanged(0);
        }

        protected virtual void EvaluateRecords()
        {
            if (_recordList == null)
                throw new ArgumentNullException("RecordList");

            RaiseStatusChanged("Controllo dei Record letti");

            IEnumerable<SyncItem<T>> evaluatedRecords = RecordEvaluator.EvaluateRecords(_recordList);

            foreach (SyncItem<T> record in evaluatedRecords)
            {
                if (!RecordValidator.IsValid(record.Item))
                    record.Action = SyncAction.Ignore;
                else
                {
                    if (record.Action == SyncAction.Insert)
                        AddRecordToInserts(record.Item);
                    else if (record.Action == SyncAction.Update)
                        AddRecordToUpdates(record.Item);
                    else if (record.Action == SyncAction.Delete)
                        AddRecordToDeletes(record.Item);
                }
            }
        }

        protected virtual void FinalizeSync()
        {
            RaisePhaseProgressChanged(100);

            RaiseStatusChanged("Pulizia della memoria");
            Clear();

            if (AbortStatus == null)
                RaiseStatusChanged("Completato");
            else
                RaiseStatusChanged("Annullato: " + AbortStatus.AbortReason);
        }

        protected virtual void Initialize()
        {
            RaiseStatusChanged("Inizializzazione");
            RaisePhaseProgressChanged(0);

            _recordsToDelete = new List<T>();
            _recordsToInsert = new List<T>();
            _recordsToUpdate = new List<T>();

            ConfigureRecordEvaluator();
            ConfigureRecordValidator();

            InitializeRecordEvaluator();
            InitializeRecordValidator();
        }

        protected abstract void ConfigureRecordEvaluator();

        protected abstract void ConfigureRecordValidator();

        protected virtual void InitializeRecordEvaluator()
        {
            RecordEvaluator.InitializeIndex(_sSMDData);
        }

        protected virtual void InitializeRecordValidator()
        {
            RecordValidator.InitializeIndexes(_sSMDData);
        }

        protected virtual void InsertNewRecords()
        {
            RaiseStatusChanged("Inserimento Nuovi record");
            RaisePhaseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(_recordsToInsert);
            insertCommand.ProgressChanged += ChangeProgress;
            _sSMDData.Execute(insertCommand);
        }

        protected virtual void OnUpdateCompleted()
        {
        }

        protected void RaisePhaseProgressChanged(int newProgress)
        {
            PhaseProgress = newProgress;
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(newProgress, null);
            ProgressChanged?.Invoke(this, e);
        }

        protected void RaiseStatusChanged(string newStatus)
        {
            SyncStatus = newStatus;
            EventArgs e = new EventArgs();
            StatusChanged?.Invoke(this, e);
        }

        protected virtual void RaiseSyncAborted()
        {
            SyncAborted?.Invoke(this, new EventArgs());
        }

        protected virtual IList<T> ReadRecordTable()
        {
            return null;
        }

        protected virtual void RetrieveSAPRecords()
        {
            RaiseStatusChanged("Recupero Record da SAP");
            RaisePhaseProgressChanged(0);

            try
            {
                _recordList = ReadRecordTable();
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveConfirmations error: " + e.Message, e);
            }
        }

        protected virtual void UpdateExistingRecords()
        {
            RaiseStatusChanged("Aggiornamento record esistenti");
            RaisePhaseProgressChanged(0);
            BatchUpdateEntitiesCommand<SSMDContext> updateCommand = new BatchUpdateEntitiesCommand<SSMDContext>(_recordsToUpdate);
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