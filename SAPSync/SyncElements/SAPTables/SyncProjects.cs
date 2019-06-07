using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class ProjectEvaluator : RecordEvaluator<Project, int>
    {
        #region Methods

        protected override int GetIndexKey(Project record) => record.ID;

        #endregion Methods
    }

    public class SyncProjects : SyncSAPTable<Project>
    {
        #region Constructors

        public SyncProjects(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Progetti";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new ProjectEvaluator();
        }

        protected override IList<Project> ReadRecordTable() => new ReadProjects().Invoke(_rfcDestination);

        #endregion Methods
    }
}