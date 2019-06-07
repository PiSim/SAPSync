using DataAccessCore;
using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class OrderDataEvaluator : RecordEvaluator<OrderData, int>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new OrderDataValidator();
        }

        protected override int GetIndexKey(OrderData record) => record.OrderNumber;

        #endregion Methods
    }

    public class OrderDataValidator : IRecordValidator<OrderData>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public OrderData GetInsertableRecord(OrderData record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number);
        }

        public bool IsValid(OrderData record) => _orderIndex.ContainsKey(record.OrderNumber);

        #endregion Methods
    }

    public class SyncOrderData : SyncSAPTable<OrderData>
    {
        #region Constructors

        public SyncOrderData(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Dati Ordine";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new OrderDataEvaluator();
        }

        protected override IList<OrderData> ReadRecordTable() => new ReadOrderData().Invoke(_rfcDestination);

        #endregion Methods
    }
}