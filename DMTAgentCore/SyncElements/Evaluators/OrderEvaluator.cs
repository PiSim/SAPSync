using SSMD;

namespace DMTAgentCore.SyncElements.Evaluators
{
    public class OrderEvaluator : RecordEvaluator<Order, int>
    {
        #region Constructors

        public OrderEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(Order record) => record.Number;

        #endregion Methods
    }
}