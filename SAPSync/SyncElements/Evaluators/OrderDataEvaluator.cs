using DataAccessCore;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.Evaluators
{
    public class OrderDataEvaluator : RecordEvaluator<OrderData, int>
    {
        #region Constructors

        public OrderDataEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(OrderData record) => record.OrderNumber;

        protected override IRecordValidator<OrderData> GetRecordValidator() => new OrderDataValidator();

        #endregion Methods
    }

    public class OrderDataValidator : IRecordValidator<OrderData>
    {
        #region Fields

        private IDictionary<string, Material> _materialDictionary;
        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null && _materialDictionary != null;

        public OrderData GetInsertableRecord(OrderData record)
        {
            record.MaterialID = _materialDictionary[record.Material.Code].ID;
            record.Material = null;
            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number);
            _materialDictionary = sSMDData.RunQuery(new MaterialsQuery()).ToDictionary(mat => mat.Code, mat => mat);
        }

        public bool IsValid(OrderData record) => _orderIndex.ContainsKey(record.OrderNumber) && _materialDictionary.ContainsKey(record.Material.Code);

        #endregion Methods
    }
}