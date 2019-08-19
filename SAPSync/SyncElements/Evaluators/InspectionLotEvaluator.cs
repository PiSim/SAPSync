using SAP.Middleware.Connector;
using SAPSync.Functions;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.Evaluators
{
    public class InspectionLotEvaluator : RecordEvaluator<InspectionLot, long>
    {
        #region Methods

        protected override IRecordValidator<InspectionLot> GetRecordValidator() => new InspectionLotValidator();
        

        protected override long GetIndexKey(InspectionLot record) => record.Number;

        #endregion Methods
    }


}