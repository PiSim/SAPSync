using SSMD;

namespace DMTAgentCore.SyncElements.Evaluators
{
    public class ProjectEvaluator : RecordEvaluator<Project, int>
    {
        #region Constructors

        public ProjectEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(Project record) => record.ID;

        #endregion Methods
    }
}