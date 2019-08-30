using SSMD;
using System;

namespace DMTAgentCore.SyncElements
{
    public class MaterialFamilyLevelEvaluator : RecordEvaluator<MaterialFamilyLevel, Tuple<int, string>>
    {
        #region Constructors

        public MaterialFamilyLevelEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

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