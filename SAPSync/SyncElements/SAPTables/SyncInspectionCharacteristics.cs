using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
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

    public class SyncInspectionCharacteristics : SyncSAPTable<InspectionCharacteristic>
    {
        #region Constructors

        public SyncInspectionCharacteristics(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Caratteristiche di Controllo";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<InspectionCharacteristic> records)
        {
        }

        protected override IRecordEvaluator<InspectionCharacteristic> GetRecordEvaluator() => new InspectionCharacteristicEvaluator();

        protected override IList<InspectionCharacteristic> ReadRecordTable() => new ReadInspectionCharacteristics().Invoke(_rfcDestination);

        #endregion Methods
    }
}