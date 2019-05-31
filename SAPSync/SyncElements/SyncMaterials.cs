using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class MaterialEvaluator : RecordEvaluator<Material, string>
    {
        #region Methods

        public override Material SetPrimaryKeyForExistingRecord(Material record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return record;
        }

        protected override string GetIndexKey(Material record) => record.Code;

        #endregion Methods
    }

    public class MaterialValidator : IRecordValidator<Material>
    {
        #region Fields

        private IDictionary<string, MaterialFamily> _familyDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _familyDictionary != null;

        public Material GetInsertableRecord(Material record)
        {
            if (record.MaterialFamily != null)
            {
                if (_familyDictionary.ContainsKey(record.MaterialFamily.FullCode))
                    record.MaterialFamilyID = _familyDictionary[record.MaterialFamily.FullCode].ID;

                record.MaterialFamily = null;
            }

            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _familyDictionary = sSMDData.RunQuery(new MaterialFamiliesQuery() { EagerLoadingEnabled = true }).ToDictionary(fam => fam.FullCode);
        }

        public bool IsValid(Material record) => record.Code[0] == '1' || record.Code[0] == '2' || record.Code[0] == '3';

        #endregion Methods
    }

    public class SyncMaterials : SyncElement<Material>
    {
        #region Constructors

        public SyncMaterials()
        {
            Name = "Materiali";
        }

        #endregion Constructors

        #region Methods

        protected override void AddRecordToUpdates(Material record)
        {
            base.AddRecordToUpdates(RecordEvaluator.SetPrimaryKeyForExistingRecord(record));
        }

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new MaterialEvaluator();
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new MaterialValidator();
        }

        protected override IList<Material> ReadRecordTable() => new ReadMaterials().Invoke(_rfcDestination);

        #endregion Methods
    }
}