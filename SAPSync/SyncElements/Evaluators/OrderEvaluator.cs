using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class OrderEvaluator : RecordEvaluator<Order, int>
    {
        #region Methods

        protected override int GetIndexKey(Order record) => record.Number;

        #endregion Methods
    }

}