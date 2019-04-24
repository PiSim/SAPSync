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

        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_inspectionCharacteristicDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Caratteristiche");
        }
        
        protected override IList<InspectionCharacteristic> ReadRecordTable() => new ReadInspectionCharacteristics().Invoke(_rfcDestination);

        protected override bool MustIgnoreRecord(InspectionCharacteristic record) => _inspectionCharacteristicDictionary.ContainsKey(record.Name);

        protected override void AddRecordToInserts(InspectionCharacteristic record)
        {
            base.AddRecordToInserts(record);
            _inspectionCharacteristicDictionary.Add(record.Name, record);
        }

        #endregion Methods
    }
}