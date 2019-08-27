using SSMD;

namespace SAPSync.SyncElements.SAPTables
{
    public class CustomerEvaluator : RecordEvaluator<Customer, int>
    {
        #region Constructors

        public CustomerEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(Customer record) => record.ID;

        #endregion Methods
    }
}