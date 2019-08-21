using DataAccessCore;
using SAPSync.RFCFunctions;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.Evaluators
{
    public class MaterialFamilyEvaluator : RecordEvaluator<MaterialFamily, string>
    {
        public MaterialFamilyEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override Query<MaterialFamily, SSMDContext> GetIndexEntriesQuery() => new MaterialFamiliesQuery() { EagerLoadingEnabled = true };

        protected override IRecordValidator<MaterialFamily> GetRecordValidator() => new MaterialFamilyValidator();

        protected override string GetIndexKey(MaterialFamily record) => record.FullCode;

        protected override MaterialFamily SetPrimaryKeyForExistingRecord(MaterialFamily record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

}