using SAP.Middleware.Connector;
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

        public SyncComponents(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Componenti";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new ComponentEvaluator();
        }

        protected override IList<Component> ReadRecordTable() => new ReadComponents().Invoke(_rfcDestination);

        #endregion Methods
    }
}