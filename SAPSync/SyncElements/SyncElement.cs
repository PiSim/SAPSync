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
        public class AbortToken
        {
            public string AbortReason { get; set; }
        }

        #region Events

        protected IList<T> _recordList;

        protected IList<T> _recordsToInsert;
        protected IList<T> _recordsToIgnore;

        protected IList<T> _recordsToUpdate;

        protected RfcDestination _rfcDestination;

        public AbortToken AbortStatus { get; set; }

        protected SSMDData _sSMDData;

        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler SyncAborted;

        public event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        public string Name { get; protected set; }
        public int PhaseProgress { get; set; }
        public bool RequiresSync { get; set; } = true;
        public string SyncStatus { get; protected set; }

        #endregion Properties

        #region Methods

        protected void Abort(string abortReason)
        {
            AbortStatus = new AbortToken() { AbortReason = abortReason };
            RaiseStatusChanged("Annullato");
            RaiseSyncAborted();
        }

        protected virtual void RaiseSyncAborted()
        {
            SyncAborted?.Invoke(this, new EventArgs());
        }

        public SyncElement()
        {
            SyncStatus = "";
        }

        public void SetOnQueue()
        {
            RaiseStatusChanged("In Coda");
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
                RaiseStatusChanged("Completato");
            }

            RaisePhaseProgressChanged(100);

            _sSMDData = null;
            _rfcDestination = null;
        }

        protected void ChangeProgress(object sender, ProgressChangedEventArgs e)
        {
            RaisePhaseProgressChanged(e.ProgressPercentage);
        }

        protected virtual bool IsNewRecord(T record) => true;

        protected virtual bool MustIgnoreRecord(T record) => true;

        protected virtual void Initialize()
        {
            RaiseStatusChanged("Inizializzazione");
            RaisePhaseProgressChanged(0);
            _recordsToIgnore = new List<T>();
            _recordsToInsert = new List<T>();
            _recordsToUpdate = new List<T>();
        }

        public bool IgnoreAllExistingRecords { get; set; } = false;

        protected virtual void EnsureInitialized()
        {
            if (_recordsToIgnore == null ||
                _recordsToInsert == null ||
                _recordsToUpdate == null)
                throw new InvalidOperationException("Liste record non inizializzate");

            if (_rfcDestination == null)
                throw new ArgumentNullException("RfcDestination");

            if (_sSMDData == null)
                throw new ArgumentNullException("SSMDData");
        }

        protected virtual void EvaluateRecords()
        {
            if (_recordList == null)
                throw new ArgumentNullException("RecordList");

            foreach (T record in _recordList)
            {
                if (MustIgnoreRecord(record))
                    _recordsToIgnore.Add(record);

                else if (IsNewRecord(record))
                    AddRecordToInserts(record);

                else if (!IgnoreAllExistingRecords)
                    AddRecordToUpdates(record);

                else
                    _recordsToIgnore.Add(record);
            }
        }

        protected virtual void AddRecordToInserts(T record) => _recordsToInsert.Add(record);

        protected virtual void AddRecordToUpdates(T record) => _recordsToUpdate.Add(record);

        protected virtual void InsertNewRecords()
        {
            RaiseStatusChanged("Inserimento Nuovi record");
            RaisePhaseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(_recordsToInsert);
            insertCommand.ProgressChanged += ChangeProgress;
            _sSMDData.Execute(insertCommand);
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

        protected virtual IList<T> ReadRecordTable()
        {
            return null;
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
    }
}