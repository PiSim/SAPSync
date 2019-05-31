using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class InspectionPointEvaluator : RecordEvaluator<InspectionPoint, Tuple<long, int, int, int>>
    {
        #region Methods

        protected override Tuple<long, int, int, int> GetIndexKey(InspectionPoint record) => record.GetPrimaryKey();

        #endregion Methods
    }

    public class InspectionPointValidator : IRecordValidator<InspectionPoint>
    {
        #region Fields

        private Dictionary<long, InspectionLot> _inspectionLotDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _inspectionLotDictionary != null;

        public InspectionPoint GetInsertableRecord(InspectionPoint record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _inspectionLotDictionary = sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(ispl => ispl.Number, ispl => ispl);
        }

        public bool IsValid(InspectionPoint record) => _inspectionLotDictionary.ContainsKey(record.InspectionLotNumber);

        #endregion Methods
    }

    public class SyncInspectionPoints : SyncElement<InspectionPoint>
    {
        #region Constructors

        public SyncInspectionPoints()
        {
            Name = "Punti di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new InspectionPointEvaluator() { IgnoreExistingRecords = true };
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new InspectionPointValidator();
        }

        protected override IList<InspectionPoint> ReadRecordTable() => new ReadInspectionPoints().Invoke(_rfcDestination);

        #endregion Methods
    }
}