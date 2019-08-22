﻿using DataAccessCore;
using SAPSync.RFCFunctions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using SAPSync.SyncElements.Validators;

namespace SAPSync.SyncElements.Evaluators
{
    public class InspectionSpecificationEvaluator : RecordEvaluator<InspectionSpecification, Tuple<long, int, int>>
    {
        public InspectionSpecificationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<InspectionSpecification> GetRecordValidator() => new InspectionSpecificationValidator();
        

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

        public bool CheckIndexesInitialized() => _characteristicDictionary != null && _lotDictionary != null;

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

}