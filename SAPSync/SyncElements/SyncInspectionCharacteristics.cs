using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class InspectionCharacteristicEvaluator : RecordEvaluator<InspectionCharacteristic, string>
    {
        #region Methods

        protected override string GetIndexKey(InspectionCharacteristic record) => record.Name;
        public override InspectionCharacteristic SetPrimaryKeyForExistingRecord(InspectionCharacteristic record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }
        #endregion Methods

    }

    public class SyncInspectionCharacteristics : SyncElement<InspectionCharacteristic>
    {
        #region Constructors

        public SyncInspectionCharacteristics()
        {
            Name = "Caratteristiche di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void AddRecordToUpdates(InspectionCharacteristic record) => base.AddRecordToUpdates(RecordEvaluator.SetPrimaryKeyForExistingRecord(record));

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new InspectionCharacteristicEvaluator() { IgnoreExistingRecords = true };
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new RecordValidator<InspectionCharacteristic>();
        }

        protected override IList<InspectionCharacteristic> ReadRecordTable() => new ReadInspectionCharacteristics().Invoke(_rfcDestination);

        #endregion Methods
    }
}