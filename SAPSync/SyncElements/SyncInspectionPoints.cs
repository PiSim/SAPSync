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
            _inspectionLotDictionary = _sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(ispl => ispl.Number, ispl => ispl);

            if (_inspectionLotDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Lotti");

            _inspectionCharacteristicDictionary = _sSMDData.RunQuery(new InspectionCharacteristicsQuery()).ToDictionary(insc => insc.Name, insc => insc);

            if (_inspectionCharacteristicDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Caratteristiche");

            _inspectionPointDictionary = _sSMDData.RunQuery(new InspectionPointsQuery()).ToDictionary(insp => insp.GetPrimaryKey(), insp => insp);

            if (_inspectionPointDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Punti");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();

            IList<InspectionPoint> inspectionPoints = RetrieveInspectionPoints(_rfcDestination);

            _recordsToInsert = new List<InspectionPoint>();
            _recordsToUpdate = new List<InspectionPoint>();

            foreach (InspectionPoint inspectionPoint in inspectionPoints)
            {
                if (!_inspectionLotDictionary.ContainsKey(inspectionPoint.InspectionLotNumber))
                    continue;

                Tuple<long, int, int, int> currentKey = inspectionPoint.GetPrimaryKey();

                if (_inspectionPointDictionary.ContainsKey(currentKey))
                    _recordsToUpdate.Add(inspectionPoint);
                else
                    _recordsToInsert.Add(inspectionPoint);
            }
        }

        private IList<InspectionPoint> RetrieveInspectionPoints(RfcDestination destination)
        {
            try
            {
                return new ReadInspectionPoints().Invoke(destination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionPoints error: " + e.Message);
            }
        }

        #endregion Methods
    }
}