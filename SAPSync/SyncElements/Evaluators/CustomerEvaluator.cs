using SAPSync.RFCFunctions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.SAPTables
{
    public class CustomerEvaluator : RecordEvaluator<Customer, int>
    {
        public CustomerEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override int GetIndexKey(Customer record) => record.ID;

        #endregion Methods
    }

}