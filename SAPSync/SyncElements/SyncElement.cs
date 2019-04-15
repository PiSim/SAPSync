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
        #region Events

        protected IList<T> _recordsToInsert;

        protected IList<T> _recordsToUpdate;

        protected RfcDestination _rfcDestination;

        protected SSMDData _sSMDData;

        public event ProgressChangedEventHandler ProgressChanged;

        public event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        public string Name { get; protected set; }
        public int PhaseProgress { get; set; }
        public bool RequiresSync { get; set; } = true;
        public string SyncStatus { get; protected set; }

        #endregion Properties

        #region Methods

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
            RetrieveSAPRecords();
            InsertNewRecords();
            UpdateExistingRecords();
            RaiseStatusChanged("Completato");
            RaisePhaseProgressChanged(100);

            _sSMDData = null;
            _rfcDestination = null;
        }

        protected void ChangeProgress(object sender, ProgressChangedEventArgs e)
        {
            RaisePhaseProgressChanged(e.ProgressPercentage);
        }

        protected virtual void Initialize()
        {
            RaiseStatusChanged("Inizializzazione");
            RaisePhaseProgressChanged(0);
            _recordsToInsert = new List<T>();
            _recordsToUpdate = new List<T>();
        }

        protected virtual void InsertNewRecords()
        {
            RaiseStatusChanged("Inserimento Nuovi record");
            RaisePhaseProgressChanged(0);
            BulkInsertEntitiesCommand<SSMDContext> insertCommand = new BulkInsertEntitiesCommand<SSMDContext>(_recordsToInsert);
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
        }

        protected virtual void UpdateExistingRecords()
        {
            RaiseStatusChanged("Aggiornamento record esistenti");
            RaisePhaseProgressChanged(0);
            BulkUpdateEntitiesCommand<SSMDContext> updateCommand = new BulkUpdateEntitiesCommand<SSMDContext>(_recordsToUpdate);
            updateCommand.ProgressChanged += ChangeProgress;
            _sSMDData.Execute(updateCommand);
        }

        #endregion Methods
    }
}