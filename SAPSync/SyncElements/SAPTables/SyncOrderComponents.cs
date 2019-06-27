using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class OrderComponentEvaluator : RecordEvaluator<OrderComponent, Tuple<int, string>>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new OrderComponentValidator();
        }

        protected override Query<OrderComponent, SSMDContext> GetIndexEntriesQuery() => new OrderComponentsQuery() { EagerLoadingEnabled = true };

        protected override Tuple<int, string> GetIndexKey(OrderComponent record) => new Tuple<int, string>(record.OrderNumber, record.Component.Name);

        protected override OrderComponent SetPrimaryKeyForExistingRecord(OrderComponent record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

    public class OrderComponentValidator : IRecordValidator<OrderComponent>
    {
        #region Fields

        private Dictionary<string, Component> _componentDictionary;
        private Dictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _componentDictionary != null && _orderDictionary != null;

        public OrderComponent GetInsertableRecord(OrderComponent record)
        {
            OrderComponent outRecord = record;
            outRecord.ComponentID = _componentDictionary[record.Component.Name].ID;
            outRecord.Component = null;
            return outRecord;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _componentDictionary = sSMDData.RunQuery(new Query<Component, SSMDContext>()).ToDictionary(mat => mat.Name, mat => mat);
            _orderDictionary = sSMDData.RunQuery(new OrdersQuery()).ToDictionary(ord => ord.Number, ord => ord);
        }

        public bool IsValid(OrderComponent record) => _orderDictionary.ContainsKey(record.OrderNumber) && _componentDictionary.ContainsKey(record.Component.Name);

        #endregion Methods
    }

    public class SyncOrderComponents : SyncSAPTable<OrderComponent>
    {
        #region Constructors

        public SyncOrderComponents(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Componenti Ordine";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<OrderComponent> records)
        {
        }

        protected override IRecordEvaluator<OrderComponent> GetRecordEvaluator() => new OrderComponentEvaluator();

        protected override IList<OrderComponent> ReadRecordTable() => new ReadOrderComponents().Invoke(_rfcDestination);

        #endregion Methods
    }
}