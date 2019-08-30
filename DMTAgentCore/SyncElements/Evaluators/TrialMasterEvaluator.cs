using DMTAgent.SyncElements.Validators;
using SSMD;

namespace DMTAgentCore.SyncElements
{
    public class TrialMasterEvaluator : RecordEvaluator<OrderData, int>
    {
        #region Constructors

        public TrialMasterEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(OrderData record) => record.OrderNumber;

        protected override IRecordValidator<OrderData> GetRecordValidator() => new TrialMasterRecordValidator();

        #endregion Methods
    }
}