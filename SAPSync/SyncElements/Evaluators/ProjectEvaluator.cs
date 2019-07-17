using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class ProjectEvaluator : RecordEvaluator<Project, int>
    {
        #region Methods

        protected override int GetIndexKey(Project record) => record.ID;

        #endregion Methods
    }

}