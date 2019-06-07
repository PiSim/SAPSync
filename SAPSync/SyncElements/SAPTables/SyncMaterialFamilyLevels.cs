using SAP.Middleware.Connector;
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

        public SyncMaterialFamilylevels(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Livelli gerarchia prodotto";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new MaterialFamilyLevelEvaluator() { IgnoreExistingRecords = true };
        }

        protected override IList<MaterialFamilyLevel> ReadRecordTable() => new ReadMaterialFamilyLevels().Invoke(_rfcDestination);

        #endregion Methods
    }
}