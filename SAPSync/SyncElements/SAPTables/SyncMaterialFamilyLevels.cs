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

        protected override MaterialFamilyLevel SetPrimaryKeyForExistingRecord(MaterialFamilyLevel record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

    public class SyncMaterialFamilylevels : SyncSAPTable<MaterialFamilyLevel>
    {
        #region Constructors

        public SyncMaterialFamilylevels(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Livelli gerarchia prodotto";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<MaterialFamilyLevel> records)
        {
        }

        protected override IRecordEvaluator<MaterialFamilyLevel> GetRecordEvaluator() => new MaterialFamilyLevelEvaluator();

        protected override IList<MaterialFamilyLevel> ReadRecordTable() => new ReadMaterialFamilyLevels().Invoke(_rfcDestination);

        #endregion Methods
    }
}