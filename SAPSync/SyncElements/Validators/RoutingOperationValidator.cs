using DataAccessCore;
using SSMD;
using System.Collections.Generic;
using System.Linq;
using SAPSync.SyncElements.Validators;

namespace SAPSync.SyncElements
{
    public class RoutingOperationValidator : IRecordValidator<RoutingOperation>
    {
        #region Fields

        private IDictionary<long, OrderData> _routingIndex;
        private IDictionary<int, WorkCenter> _workCenterIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _workCenterIndex != null && _routingIndex != null;

        public RoutingOperation GetInsertableRecord(RoutingOperation record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _workCenterIndex = sSMDData.RunQuery(new Query<WorkCenter, SSMDContext>()).ToDictionary(wkc => wkc.ID, wkc => wkc);
            _routingIndex = sSMDData.RunQuery(new Query<OrderData, SSMDContext>()).ToDictionary(ord => ord.RoutingNumber, ord => ord);
        }

        public bool IsValid(RoutingOperation record) => _workCenterIndex.ContainsKey(record.WorkCenterID) && _routingIndex.ContainsKey(record.RoutingNumber);

        #endregion Methods
    }
}