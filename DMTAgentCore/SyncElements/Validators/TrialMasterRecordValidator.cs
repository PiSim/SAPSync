using DataAccessCore;
using DMTAgent.SyncElements.Validators;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgentCore.SyncElements
{
    public class TrialMasterRecordValidator : IRecordValidator<OrderData>
    {
        #region Fields

        private IDictionary<int, OrderData> _orderDataIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderDataIndex != null;

        public OrderData GetInsertableRecord(OrderData record)
        {
            OrderData existingRecord = _orderDataIndex[record.OrderNumber];
            existingRecord.HasSampleArrived = record.HasSampleArrived;
            existingRecord.SampleArrivalDate = record.SampleArrivalDate;
            existingRecord.SampleRollStatus = record.SampleRollStatus;
            return existingRecord;
        }

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
            _orderDataIndex = sSMDData.RunQuery(new Query<OrderData, SSMDContext>()).ToDictionary(od => od.OrderNumber, od => od);
        }

        public bool IsValid(OrderData record) => _orderDataIndex.ContainsKey(record.OrderNumber);

        #endregion Methods
    }
}