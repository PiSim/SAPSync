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
            if (_lotDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario Lotti");

            _specificationDictionary = _sSMDData.RunQuery(new Query<InspectionSpecification, SSMDContext>()).ToDictionary(insps => insps.GetPrimaryKey(), insps => insps);
            if (_lotDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario Specifiche");

            _characteristicDictionary = _sSMDData.RunQuery(new Query<InspectionCharacteristic, SSMDContext>()).ToDictionary(inspc => inspc.Name, inspc => inspc);

            if (_characteristicDictionary == null)
                throw new Exception("Impossibile recuperare il dizionario caratteristiche");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();
            IList<Tuple<string, InspectionSpecification>> inspectionSpecifications = GetInspectionSpecifications();

            foreach (Tuple<string, InspectionSpecification> specData in inspectionSpecifications)
            {
                InspectionSpecification specRecord = specData.Item2;

                if (!_characteristicDictionary.ContainsKey(specData.Item1))
                    continue;

                specRecord.InspectionCharacteristicID = _characteristicDictionary[specData.Item1].ID;

                if (_specificationDictionary.ContainsKey(specRecord.GetPrimaryKey()))
                    _recordsToUpdate.Add(specRecord);
                else
                {
                    if (!_lotDictionary.ContainsKey(specRecord.InspectionLotNumber))
                        continue;

                    _recordsToInsert.Add(specRecord);
                }
            }
        }

        private IList<Tuple<string, InspectionSpecification>> GetInspectionSpecifications()
        {
            try
            {
                return (new ReadInspectionSpecifications()).Invoke(_rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("Impossible recuperare i record InspectionSpecification", e);
            }
        }

        #endregion Methods
    }
}