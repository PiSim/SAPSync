using SSMD;

namespace DMTAgent.SyncElements.Evaluators
{
    public class ComponentEvaluator : RecordEvaluator<Component, string>
    {
        #region Constructors

        public ComponentEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override string GetIndexKey(Component record) => record.Name;

        protected override Component SetPrimaryKeyForExistingRecord(Component record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }
}