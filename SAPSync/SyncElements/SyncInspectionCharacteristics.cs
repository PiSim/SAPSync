using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncInspectionCharacteristics : SyncElement<InspectionCharacteristic>
    {
        #region Fields

        private IDictionary<string, InspectionCharacteristic> _inspectionCharacteristicDictionary;

        #endregion Fields

        #region Constructors

        public SyncInspectionCharacteristics()
        {
            Name = "Caratteristiche di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();

            _inspectionCharacteristicDictionary = _sSMDData.RunQuery(new InspectionCharacteristicsQuery()).ToDictionary(insc => insc.Name, insc => insc);

            if (_inspectionCharacteristicDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Caratteristiche");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();
            IList<InspectionCharacteristic> charTable = RetrieveInspectionCharacteristics(_rfcDestination);
            _recordsToInsert = GetValidatedCharacteristics(charTable);
        }

        private IList<InspectionCharacteristic> GetValidatedCharacteristics(IEnumerable<InspectionCharacteristic> inspectionCharacteristics)
        {
            IList<InspectionCharacteristic> output = new List<InspectionCharacteristic>();

            foreach (InspectionCharacteristic currentCharacteristic in inspectionCharacteristics)
            {
                if (_inspectionCharacteristicDictionary.ContainsKey(currentCharacteristic.Name))
                    continue;

                output.Add(currentCharacteristic);
                _inspectionCharacteristicDictionary.Add(currentCharacteristic.Name, currentCharacteristic);
            }

            return output;
        }

        private IList<InspectionCharacteristic> RetrieveInspectionCharacteristics(RfcDestination rfcDestination)
        {
            IList<InspectionCharacteristic> output;

            try
            {
                output = new ReadInspectionCharacteristics().Invoke(rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionCharacteristics error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}