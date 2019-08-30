using DataAccessCore;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgentCore.SyncElements.Validators
{
    public class TestReportRecordValidator : IRecordValidator<TestReport>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public TestReport GetInsertableRecord(TestReport record) => record;

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number, ord => ord);
        }

        public bool IsValid(TestReport record) => record.OrderNumber == null || _orderIndex.ContainsKey((int)record.OrderNumber);

        #endregion Methods
    }
}