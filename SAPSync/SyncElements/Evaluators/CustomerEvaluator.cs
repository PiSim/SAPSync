using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.SAPTables
{
    public class CustomerEvaluator : RecordEvaluator<Customer, int>
    {
        #region Methods

        protected override int GetIndexKey(Customer record) => record.ID;

        #endregion Methods
    }

}