using DataAccessCore;
using SSMD;
using SSMD.Queries;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgentCore.SyncElements.Validators
{
    public class InspectionLotValidator : IRecordValidator<InspectionLot>
    {
        #region Fields

        private IDictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderDictionary != null;

        public InspectionLot GetInsertableRecord(InspectionLot record) => record;

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
            _orderDictionary = sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);
        }

        public bool IsValid(InspectionLot record) => _orderDictionary.ContainsKey(record.OrderNumber);

        #endregion Methods
    }
}