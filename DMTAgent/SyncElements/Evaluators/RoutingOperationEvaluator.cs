using DMTAgent.SyncElements.Validators;
using SSMD;
using System;

namespace DMTAgent.SyncElements.Evaluators
{
    public class RoutingOperationEvaluator : RecordEvaluator<RoutingOperation, Tuple<long, int>>
    {
        #region Constructors

        public RoutingOperationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Tuple<long, int> GetIndexKey(RoutingOperation record) => new Tuple<long, int>(record.RoutingNumber, record.RoutingCounter);

        protected override IRecordValidator<RoutingOperation> GetRecordValidator() => new RoutingOperationValidator();

        #endregion Methods
    }
}