using DataAccessCore;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent.SyncElements.Validators
{
    public class GoodMovementValidator : IRecordValidator<GoodMovement>
    {
        #region Fields

        private IDictionary<string, Component> _componentIndex;
        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null && _componentIndex != null;

        public GoodMovement GetInsertableRecord(GoodMovement record)
        {
            record.ComponentID = _componentIndex[record.Component.Name].ID;
            record.Component = null;
            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number, ord => ord);
            _componentIndex = sSMDData.RunQuery(new Query<Component, SSMDContext>()).ToDictionary(comp => comp.Name, mat => mat);
        }

        public bool IsValid(GoodMovement record) => _orderIndex.ContainsKey(record.OrderNumber)
            && record.Component != null
            && _componentIndex.ContainsKey(record.Component.Name);

        #endregion Methods
    }
}