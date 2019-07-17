using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.Evaluators
{
    public class ComponentEvaluator : RecordEvaluator<Component, string>
    {
        #region Methods

        protected override string GetIndexKey(Component record) => record.Name;

        protected override Component SetPrimaryKeyForExistingRecord(Component record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

}