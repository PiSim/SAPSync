using SAPSync.SyncElements.Validators;
using SSMD;
using System;

namespace SAPSync.SyncElements.Evaluators
{
    public class GoodMovementEvaluator : RecordEvaluator<GoodMovement, Tuple<long, int>>
    {
        #region Constructors

        public GoodMovementEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Tuple<long, int> GetIndexKey(GoodMovement record) => record.GetPrimaryKey();

        protected override IRecordValidator<GoodMovement> GetRecordValidator() => new GoodMovementValidator();

        #endregion Methods
    }
}