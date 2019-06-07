using DataAccessCore;
using SAP.Middleware.Connector;
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

        public override void Initialize(SSMDData sSMDData)
        {
            _recordIndex = sSMDData.RunQuery(new OrderComponentsQuery() { EagerLoadingEnabled = true }).ToDictionary(rec => GetIndexKey(rec), rec => rec);
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new OrderComponentValidator();
        }

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

        public SyncOrderComponents(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Componenti Ordine";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new OrderComponentEvaluator();
        }

        protected override IList<OrderComponent> ReadRecordTable() => new ReadOrderComponents().Invoke(_rfcDestination);

        #endregion Methods
    }
}