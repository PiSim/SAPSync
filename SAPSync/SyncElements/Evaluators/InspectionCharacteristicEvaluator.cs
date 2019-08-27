using SSMD;

namespace SAPSync.SyncElements.Evaluators
{
    public class InspectionCharacteristicEvaluator : RecordEvaluator<InspectionCharacteristic, string>
    {
        #region Constructors

        public InspectionCharacteristicEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override string GetIndexKey(InspectionCharacteristic record) => record.Name;

        protected override InspectionCharacteristic SetPrimaryKeyForExistingRecord(InspectionCharacteristic record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }
}