using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Validators
{
    public class TestReportRecordValidator : IRecordValidator<TestReport>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public TestReport GetInsertableRecord(TestReport record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number, ord => ord);
        }

        public bool IsValid(TestReport record) => record.OrderNumber == null || _orderIndex.ContainsKey((int)record.OrderNumber);

        #endregion Methods
    }
}
