using SAP.Middleware.Connector;
using SAPSync.SyncElements;
using System;
using System.Collections.Generic;

namespace SAPSync
{
    public abstract class SyncSAPTable<T> : SyncElement<T> where T : class
    {
        #region Fields

        protected RfcDestination _rfcDestination;

        #endregion Fields

        #region Constructors

        public SyncSAPTable(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Clear()
        {
            _rfcDestination = null;
            base.Clear();
        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_rfcDestination == null)
                throw new ArgumentNullException("RfcDestination");
        }

        protected override void Initialize()
        {
            base.Initialize();
            _rfcDestination = (new SAPReader()).GetRfcDestination();
        }

        protected override IEnumerable<T> ReadRecords()
        {
            ChangeStatus("Recupero Record da SAP");
            RaisePhaseProgressChanged(0);

            IEnumerable<T> records;

            try
            {
                records = ReadRecordTable();
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveConfirmations error: " + e.Message, e);
            }

            return records;
        }

        protected abstract IList<T> ReadRecordTable();

        #endregion Methods
    }
}