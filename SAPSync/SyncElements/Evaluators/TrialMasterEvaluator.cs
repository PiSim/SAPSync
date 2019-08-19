using SSMD;

namespace SAPSync.SyncElements
{
    public class TrialMasterEvaluator : RecordEvaluator<OrderData, int>
    {
        public TrialMasterEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<OrderData> GetRecordValidator() => new TrialMasterRecordValidator();

        protected override int GetIndexKey(OrderData record) => record.OrderNumber;

        #endregion Methods
    }
}