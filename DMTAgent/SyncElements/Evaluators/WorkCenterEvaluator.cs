using SSMD;

namespace DMTAgent.SyncElements.Evaluators
{
    public class WorkCenterEvaluator : RecordEvaluator<WorkCenter, int>
    {
        #region Constructors

        public WorkCenterEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(WorkCenter record) => record.ID;

        #endregion Methods
    }
}