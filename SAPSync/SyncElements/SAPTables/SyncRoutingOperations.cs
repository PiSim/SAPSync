using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class RoutingOperationEvaluator : RecordEvaluator<RoutingOperation, Tuple<long, int>>
    {
        #region Methods

        protected override Tuple<long, int> GetIndexKey(RoutingOperation record) => new Tuple<long, int>(record.RoutingNumber, record.RoutingCounter);

        #endregion Methods
    }

    public class SyncRoutingOperations : SyncSAPTable<RoutingOperation>
    {
        #region Constructors

        public SyncRoutingOperations(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Operazioni Ordine";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new RoutingOperationEvaluator();
        }

        protected override IList<RoutingOperation> ReadRecordTable() => (new ReadRoutingOperations()).Invoke(_rfcDestination);

        #endregion Methods
    }
}