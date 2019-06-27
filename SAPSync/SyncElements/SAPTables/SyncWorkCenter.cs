using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class SyncWorkCenters : SyncSAPTable<WorkCenter>
    {
        #region Constructors

        public SyncWorkCenters(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Centri Di Lavoro";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<WorkCenter> records)
        {
        }

        protected override IRecordEvaluator<WorkCenter> GetRecordEvaluator() => new WorkCenterEvaluator();

        protected override IList<WorkCenter> ReadRecordTable() => new ReadWorkCenters().Invoke(_rfcDestination);

        #endregion Methods
    }

    public class WorkCenterEvaluator : RecordEvaluator<WorkCenter, int>
    {
        #region Methods

        protected override int GetIndexKey(WorkCenter record) => record.ID;

        #endregion Methods
    }
}