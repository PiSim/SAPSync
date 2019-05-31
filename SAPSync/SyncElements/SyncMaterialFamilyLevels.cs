using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class MaterialFamilyLevelEvaluator : RecordEvaluator<MaterialFamilyLevel, Tuple<int, string>>
    {
        #region Methods

        protected override Tuple<int, string> GetIndexKey(MaterialFamilyLevel record) => new Tuple<int, string>(record.Level, record.Code);

        public override MaterialFamilyLevel SetPrimaryKeyForExistingRecord(MaterialFamilyLevel record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

    public class SyncMaterialFamilylevels : SyncElement<MaterialFamilyLevel>
    {
        #region Constructors

        public SyncMaterialFamilylevels()
        {
            Name = "Livelli gerarchia prodotto";
        }

        #endregion Constructors

        #region Methods

        protected override void AddRecordToUpdates(MaterialFamilyLevel record) => base.AddRecordToUpdates(RecordEvaluator.SetPrimaryKeyForExistingRecord(record));
        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new MaterialFamilyLevelEvaluator() { IgnoreExistingRecords = true };
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new RecordValidator<MaterialFamilyLevel>();
        }

        protected override IList<MaterialFamilyLevel> ReadRecordTable() => new ReadMaterialFamilyLevels().Invoke(_rfcDestination);

        #endregion Methods
    }
}