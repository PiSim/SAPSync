using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
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

    public class SyncComponents : SyncSAPTable<Component>
    {
        #region Constructors

        public SyncComponents(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Componenti";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<Component> records)
        {
        }

        protected override IRecordEvaluator<Component> GetRecordEvaluator() => new ComponentEvaluator();

        protected override IList<Component> ReadRecordTable() => new ReadComponents().Invoke(_rfcDestination);

        #endregion Methods
    }
}