using DataAccessCore;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;

namespace SAPSync.SyncElements.Evaluators
{
    public class MaterialFamilyEvaluator : RecordEvaluator<MaterialFamily, string>
    {
        #region Constructors

        public MaterialFamilyEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Query<MaterialFamily, SSMDContext> GetIndexEntriesQuery() => new MaterialFamiliesQuery() { EagerLoadingEnabled = true };

        protected override string GetIndexKey(MaterialFamily record) => record.FullCode;

        protected override IRecordValidator<MaterialFamily> GetRecordValidator() => new MaterialFamilyValidator();

        protected override MaterialFamily SetPrimaryKeyForExistingRecord(MaterialFamily record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }
}