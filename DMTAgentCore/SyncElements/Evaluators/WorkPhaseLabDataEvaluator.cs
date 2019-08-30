using DMTAgent.SyncElements.Validators;
using SSMD;

namespace DMTAgentCore.SyncElements.Evaluators
{
    public class WorkPhaseLabDataEvaluator : RecordEvaluator<WorkPhaseLabData, int>
    {
        #region Constructors

        public WorkPhaseLabDataEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(WorkPhaseLabData record) => record.OrderNumber;

        protected override IRecordValidator<WorkPhaseLabData> GetRecordValidator() => new WorkPhaseLabDataValidator();

        #endregion Methods
    }
}