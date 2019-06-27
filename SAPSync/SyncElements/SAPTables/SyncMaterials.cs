using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAPSync.SyncElements
{
    public class MaterialEvaluator : RecordEvaluator<Material, string>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new MaterialValidator();
        }

        protected override string GetIndexKey(Material record) => record.Code;

        protected override Material SetPrimaryKeyForExistingRecord(Material record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return record;
        }

        #endregion Methods
    }

    public class MaterialValidator : IRecordValidator<Material>
    {
        #region Fields

        private IDictionary<string, MaterialFamily> _familyDictionary;
        private IDictionary<string, Project> _projectIndex;
        private IDictionary<string, Component> _splColorIndex, _calColorIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _familyDictionary != null && _projectIndex != null && _splColorIndex != null && _calColorIndex != null;

        public Material GetInsertableRecord(Material record)
        {
            if (record.MaterialFamily != null)
            {
                if (_familyDictionary.ContainsKey(record.MaterialFamily.FullCode))
                    record.MaterialFamilyID = _familyDictionary[record.MaterialFamily.FullCode].ID;

                if (_projectIndex.ContainsKey(record.Project?.Code))
                    record.ProjectID = _projectIndex[record.Project?.Code].ID;

                if (record.Code.Length >= 15 && record.Code[0] == '3' && _splColorIndex.ContainsKey(record.Code.Substring(10, 4)))
                    record.ColorComponentID = _splColorIndex[record.Code.Substring(10, 4)].ID;

                if (record.Code.Length >= 15 && record.Code[0] == '1' && _calColorIndex.ContainsKey(record.Code.Substring(10, 4)))
                    record.ColorComponentID = _calColorIndex[record.Code.Substring(10, 4)].ID;

                record.MaterialFamily = null;
                record.Project = null;
            }

            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _familyDictionary = sSMDData.RunQuery(new MaterialFamiliesQuery() { EagerLoadingEnabled = true }).ToDictionary(fam => fam.FullCode);
            _projectIndex = sSMDData.RunQuery(new Query<Project, SSMDContext>()).ToDictionary(prj => prj.Code);

            _splColorIndex = new Dictionary<string, Component>();
            foreach (Component com in sSMDData.RunQuery(new Query<Component, SSMDContext>())
                .Where(com => com.Name.Length >= 12 && Regex.IsMatch(com.Name, "^PMP110(SK|ES)")))
                if (!_splColorIndex.ContainsKey(com.Name.Substring(8, 4)))
                    _splColorIndex.Add(com.Name.Substring(8, 4), com);

            _calColorIndex = new Dictionary<string, Component>();
            foreach (Component com in sSMDData.RunQuery(new Query<Component, SSMDContext>())
                .Where(com => com.Name.Length >= 12 && Regex.IsMatch(com.Name, "^PMP060(CL|ED)")))
                if (!_calColorIndex.ContainsKey(com.Name.Substring(8, 4)))
                    _calColorIndex.Add(com.Name.Substring(8, 4), com);
        }

        public bool IsValid(Material record) => record.Code[0] == '1' || record.Code[0] == '2' || record.Code[0] == '3';

        #endregion Methods
    }

    public class SyncMaterials : SyncSAPTable<Material>
    {
        #region Constructors

        public SyncMaterials(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Materiali";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<Material> records)
        {
        }

        protected override IRecordEvaluator<Material> GetRecordEvaluator() => new MaterialEvaluator();

        protected override IList<Material> ReadRecordTable() => new ReadMaterials().Invoke(_rfcDestination);

        #endregion Methods
    }
}