using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Validators
{
    public class GoodMovementValidator : IRecordValidator<GoodMovement>
    {
        private IDictionary<int, Order> _orderIndex;
        private IDictionary<string, Component> _componentIndex;

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
    }
}
