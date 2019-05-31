using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class ComponentEvaluator : RecordEvaluator<Component, string>
    {
        #region Methods

        protected override string GetIndexKey(Component record) => record.Name;

        public override Component SetPrimaryKeyForExistingRecord(Component record)
        {
            record.ID = RecordIndex[GetIndexKey(record)].ID;
            return base.SetPrimaryKeyForExistingRecord(record);
        }

        #endregion Methods
    }

    public class SyncComponents : SyncElement<Component>
    {
        #region Constructors

        public SyncComponents()
        {
            Name = "Componenti";
        }

        #endregion Constructors

        #region Methods

        protected override void AddRecordToUpdates(Component record) => base.AddRecordToUpdates(RecordEvaluator.SetPrimaryKeyForExistingRecord(record));

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new ComponentEvaluator();
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new RecordValidator<Component>();
        }

        protected override IList<Component> ReadRecordTable() => new ReadComponents().Invoke(_rfcDestination);

        #endregion Methods
    }
}