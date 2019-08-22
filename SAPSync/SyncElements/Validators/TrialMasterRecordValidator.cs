using DataAccessCore;
using SSMD;
using System.Collections.Generic;
using System.Linq;
using SAPSync.SyncElements.Validators;

namespace SAPSync.SyncElements
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

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderDataIndex = sSMDData.RunQuery(new Query<OrderData, SSMDContext>()).ToDictionary(od => od.OrderNumber, od => od);
        }

        public bool IsValid(OrderData record) => _orderDataIndex.ContainsKey(record.OrderNumber);

        #endregion Methods
    }
}