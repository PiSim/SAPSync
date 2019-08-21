using SAPSync.RFCFunctions;
using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class MaterialFamilyLevelEvaluator : RecordEvaluator<MaterialFamilyLevel, Tuple<int, string>>
    {
        public MaterialFamilyLevelEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override Tuple<int, string> GetIndexKey(MaterialFamilyLevel record) => new Tuple<int, string>(record.Level, record.Code);

        protected override MaterialFamilyLevel SetPrimaryKeyForExistingRecord(MaterialFamilyLevel record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }
}