using DataAccessCore;
using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class MaterialFamilyEvaluator : RecordEvaluator<MaterialFamily, string>
    {
        #region Methods

        public override void Initialize(SSMDData sSMDData)
        {
            _recordIndex = sSMDData.RunQuery(new MaterialFamiliesQuery() { EagerLoadingEnabled = true }).ToDictionary(mf => GetIndexKey(mf));
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new MaterialFamilyValidator();
        }

        protected override string GetIndexKey(MaterialFamily record) => record.FullCode;

        protected override MaterialFamily SetPrimaryKeyForExistingRecord(MaterialFamily record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

    public class MaterialFamilyValidator : IRecordValidator<MaterialFamily>
    {
        #region Fields

        private IDictionary<Tuple<int, string>, MaterialFamilyLevel> _levelDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _levelDictionary != null;

        public MaterialFamily GetInsertableRecord(MaterialFamily record)
        {
            record.L1ID = _levelDictionary[record.L1.GetIndexKey()].ID;
            record.L2ID = _levelDictionary[record.L2.GetIndexKey()].ID;
            record.L3ID = _levelDictionary[record.L3.GetIndexKey()].ID;

            record.L1 = null;
            record.L2 = null;
            record.L3 = null;

            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _levelDictionary = sSMDData.RunQuery(new Query<MaterialFamilyLevel, SSMDContext>()).ToDictionary(mfl => new Tuple<int, string>(mfl.Level, mfl.Code));
        }

        public bool IsValid(MaterialFamily record) => _levelDictionary.ContainsKey(record.L1?.GetIndexKey())
            && _levelDictionary.ContainsKey(record.L2?.GetIndexKey())
            && _levelDictionary.ContainsKey(record.L3?.GetIndexKey());

        #endregion Methods
    }

    public class SyncMaterialFamilies : SyncSAPTable<MaterialFamily>
    {
        #region Constructors

        public SyncMaterialFamilies(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Gerarchia prodotto";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new MaterialFamilyEvaluator() { IgnoreExistingRecords = true };
        }

        protected override IList<MaterialFamily> ReadRecordTable() => new ReadMaterialFamilies().Invoke(_rfcDestination);

        #endregion Methods
    }
}