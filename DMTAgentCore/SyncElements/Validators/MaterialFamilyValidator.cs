using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgentCore.SyncElements.Validators
{
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

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
            _levelDictionary = sSMDData.RunQuery(new Query<MaterialFamilyLevel, SSMDContext>()).ToDictionary(mfl => new Tuple<int, string>(mfl.Level, mfl.Code));
        }

        public bool IsValid(MaterialFamily record) => _levelDictionary.ContainsKey(record.L1?.GetIndexKey())
            && _levelDictionary.ContainsKey(record.L2?.GetIndexKey())
            && _levelDictionary.ContainsKey(record.L3?.GetIndexKey());

        #endregion Methods
    }
}