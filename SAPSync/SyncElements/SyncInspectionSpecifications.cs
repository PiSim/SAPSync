using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class SyncInspectionSpecifications : SyncElement<InspectionSpecification>
    {
        #region Fields

        private IDictionary<string, InspectionCharacteristic> _characteristicDictionary;
        private IDictionary<long, InspectionLot> _lotDictionary;
        private IDictionary<Tuple<long, int, int>, InspectionSpecification> _specificationDictionary;

        #endregion Fields

        #region Constructors

        public SyncInspectionSpecifications() : base()
        {
            Name = "Specifiche di controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();

            _lotDictionary = _sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(inspl => inspl.Number, inspl => inspl);

            _specificationDictionary = _sSMDData.RunQuery(new Query<InspectionSpecification, SSMDContext>()).ToDictionary(insps => insps.GetPrimaryKey(), insps => insps);

            _characteristicDictionary = _sSMDData.RunQuery(new Query<InspectionCharacteristic, SSMDContext>()).ToDictionary(inspc => inspc.Name, inspc => inspc);

        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_lotDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario Lotti");
            if (_lotDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario Specifiche");
            if (_characteristicDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario caratteristiche");
        }

        protected override void AddRecordToInserts(InspectionSpecification record)
        {
            record.InspectionCharacteristicID = _characteristicDictionary[record.InspectionCharacteristic.Name].ID;
            record.InspectionCharacteristic = null;
            base.AddRecordToInserts(record);
        }

        protected override void AddRecordToUpdates(InspectionSpecification record)
        {
            record.InspectionCharacteristicID = _characteristicDictionary[record.InspectionCharacteristic.Name].ID;
            record.InspectionCharacteristic = null;
            base.AddRecordToUpdates(record);
        }

        protected override bool MustIgnoreRecord(InspectionSpecification record) => !_characteristicDictionary.ContainsKey(record.InspectionCharacteristic?.Name) 
            || !_lotDictionary.ContainsKey(record.InspectionLotNumber);

        protected override bool IsNewRecord(InspectionSpecification record) => !_specificationDictionary.ContainsKey(record.GetPrimaryKey());

        protected override IList<InspectionSpecification> ReadRecordTable() => new ReadInspectionSpecifications().Invoke(_rfcDestination);


        #endregion Methods
    }
}