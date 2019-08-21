using SAPSync.RFCFunctions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class ProjectEvaluator : RecordEvaluator<Project, int>
    {
        public ProjectEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override int GetIndexKey(Project record) => record.ID;

        #endregion Methods
    }

}