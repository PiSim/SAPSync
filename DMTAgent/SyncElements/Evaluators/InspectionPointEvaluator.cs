using DMTAgent.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent.SyncElements.Evaluators
{
    public class InspectionPointEvaluator : RecordEvaluator<InspectionPoint, Tuple<long, int, int, int>>
    {
        #region Constructors

        public InspectionPointEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Tuple<long, int, int, int> GetIndexKey(InspectionPoint record) => record.GetPrimaryKey();

        protected override IRecordValidator<InspectionPoint> GetRecordValidator() => new InspectionPointValidator();

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
}