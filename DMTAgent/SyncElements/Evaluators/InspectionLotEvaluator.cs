using DMTAgent.SyncElements.Validators;
using SSMD;

namespace DMTAgent.SyncElements.Evaluators
{
    public class InspectionLotEvaluator : RecordEvaluator<InspectionLot, long>
    {
        #region Constructors

        public InspectionLotEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override long GetIndexKey(InspectionLot record) => record.Number;

        protected override IRecordValidator<InspectionLot> GetRecordValidator() => new InspectionLotValidator();

        #endregion Methods
    }
}