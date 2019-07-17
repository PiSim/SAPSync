using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class InspectionCharacteristicEvaluator : RecordEvaluator<InspectionCharacteristic, string>
    {
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