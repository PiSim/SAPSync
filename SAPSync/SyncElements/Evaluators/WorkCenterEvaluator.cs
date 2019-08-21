using SAPSync.RFCFunctions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class WorkCenterEvaluator : RecordEvaluator<WorkCenter, int>
    {
        public WorkCenterEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override int GetIndexKey(WorkCenter record) => record.ID;

        #endregion Methods
    }
}