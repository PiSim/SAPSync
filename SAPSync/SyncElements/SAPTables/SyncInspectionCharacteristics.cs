using SAP.Middleware.Connector;
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

        public SyncInspectionCharacteristics(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Caratteristiche di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new InspectionCharacteristicEvaluator() { IgnoreExistingRecords = true };
        }

        protected override IList<InspectionCharacteristic> ReadRecordTable() => new ReadInspectionCharacteristics().Invoke(_rfcDestination);

        #endregion Methods
    }
}