using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.Evaluators
{
    public class RoutingOperationEvaluator : RecordEvaluator<RoutingOperation, Tuple<long, int>>
    {
        public RoutingOperationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<RoutingOperation> GetRecordValidator() => new RoutingOperationValidator();

        protected override Tuple<long, int> GetIndexKey(RoutingOperation record) => new Tuple<long, int>(record.RoutingNumber, record.RoutingCounter);

        #endregion Methods
    }

}