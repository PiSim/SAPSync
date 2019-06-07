using SAP.Middleware.Connector;
using SAPSync.SyncElements;
using SSMD;
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

        public SyncSAPTable(RfcDestination rfcDestination, SSMDData sSMDData) : base(sSMDData)
        {
            _rfcDestination = rfcDestination;
        }

        #endregion Constructors

        #region Methods

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_rfcDestination == null)
                throw new ArgumentNullException("RfcDestination");
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