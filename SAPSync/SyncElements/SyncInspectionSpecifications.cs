using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class InspectionSpecificationEvaluator : RecordEvaluator<InspectionSpecification, Tuple<long, int, int>>
    {
        #region Methods

        protected override Tuple<long, int, int> GetIndexKey(InspectionSpecification record) => record.GetPrimaryKey();

        #endregion Methods
    }

    public class InspectionSpecificationValidator : IRecordValidator<InspectionSpecification>
    {
        #region Fields

        private IDictionary<string, InspectionCharacteristic> _characteristicDictionary;
        private IDictionary<long, InspectionLot> _lotDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized()
        {
            throw new NotImplementedException();
        }

        public InspectionSpecification GetInsertableRecord(InspectionSpecification record)
        {
            record.InspectionCharacteristicID = _characteristicDictionary[record.InspectionCharacteristic.Name].ID;
            record.InspectionCharacteristic = null;
            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _lotDictionary = sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(inspl => inspl.Number, inspl => inspl);

            _characteristicDictionary = sSMDData.RunQuery(new Query<InspectionCharacteristic, SSMDContext>()).ToDictionary(inspc => inspc.Name, inspc => inspc);
        }

        public bool IsValid(InspectionSpecification record) => _characteristicDictionary.ContainsKey(record.InspectionCharacteristic?.Name)
                    && _lotDictionary.ContainsKey(record.InspectionLotNumber);

        #endregion Methods
    }

    public class SyncInspectionSpecifications : SyncElement<InspectionSpecification>
    {
        #region Constructors

        public SyncInspectionSpecifications() : base()
        {
            Name = "Specifiche di controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new InspectionSpecificationEvaluator();
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new InspectionSpecificationValidator();
        }

        protected override IList<InspectionSpecification> ReadRecordTable() => new ReadInspectionSpecifications().Invoke(_rfcDestination);

        #endregion Methods
    }
}