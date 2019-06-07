using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class SyncWorkCenters : SyncSAPTable<WorkCenter>
    {
        #region Constructors

        public SyncWorkCenters(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Centri Di Lavoro";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new WorkCenterEvaluator() { IgnoreExistingRecords = true };
        }

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