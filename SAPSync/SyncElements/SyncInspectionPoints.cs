using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncInspectionPoints : SyncElement<InspectionPoint>
    {
        #region Constructors

        private Dictionary<string, InspectionCharacteristic> _inspectionCharacteristicDictionary;
        private Dictionary<long, InspectionLot> _inspectionLotDictionary;
        private Dictionary<Tuple<long, int, int, int>, InspectionPoint> _inspectionPointDictionary;

        public SyncInspectionPoints()
        {
            Name = "Punti di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            _inspectionLotDictionary = _sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(ispl => ispl.Number, ispl => ispl);


            _inspectionCharacteristicDictionary = _sSMDData.RunQuery(new InspectionCharacteristicsQuery()).ToDictionary(insc => insc.Name, insc => insc);


            _inspectionPointDictionary = _sSMDData.RunQuery(new InspectionPointsQuery()).ToDictionary(insp => insp.GetPrimaryKey(), insp => insp);

        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_inspectionLotDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Lotti");
            if (_inspectionCharacteristicDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Caratteristiche");
            if (_inspectionPointDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Punti");
        }

        protected override bool MustIgnoreRecord(InspectionPoint record) => !_inspectionLotDictionary.ContainsKey(record.InspectionLotNumber);

        protected override bool IsNewRecord(InspectionPoint record)
        {
            Tuple<long, int, int, int> currentKey = record.GetPrimaryKey();

            return !_inspectionPointDictionary.ContainsKey(currentKey);
        }

        protected override IList<InspectionPoint> ReadRecordTable() => new ReadInspectionPoints().Invoke(_rfcDestination);

        #endregion Methods
    }
}