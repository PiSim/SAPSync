using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.Evaluators
{
    public class WBSRelationEvaluator : RecordEvaluator<WBSRelation, int>
    {
        public WBSRelationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<WBSRelation> GetRecordValidator() => new WBSRelationValidator();
        

        protected override int GetIndexKey(WBSRelation record) => record.ID;

        #endregion Methods
    }

    public class WBSRelationValidator : IRecordValidator<WBSRelation>
    {
        #region Fields

        private IDictionary<int, Project> _projectIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _projectIndex != null;

        public WBSRelation GetInsertableRecord(WBSRelation record)
        {
            if (record.UpID == 0)
                record.UpID = null;

            if (record.LeftID == 0)
                record.LeftID = null;

            if (record.RightID == 0)
                record.RightID = null;

            if (record.DownID == 0)
                record.DownID = null;

            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _projectIndex = sSMDData.RunQuery(new Query<Project, SSMDContext>()).ToDictionary(prj => prj.ID, prj => prj);
        }

        public bool IsValid(WBSRelation record)
        {
            if (!_projectIndex.ContainsKey(record.ProjectID))
                return false;

            if (record.DownID != 0 && record.DownID != null)
                if (!_projectIndex.ContainsKey((int)record.DownID))
                    return false;

            if (record.DownID != 0 && record.RightID != null)
                if (!_projectIndex.ContainsKey((int)record.DownID))
                    return false;

            return true;
        }

        #endregion Methods
    }
}