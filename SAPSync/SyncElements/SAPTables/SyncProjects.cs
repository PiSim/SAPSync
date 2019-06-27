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

        public SyncProjects(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Progetti";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<Project> records)
        {
        }

        protected override IRecordEvaluator<Project> GetRecordEvaluator() => new ProjectEvaluator();

        protected override IList<Project> ReadRecordTable() => new ReadProjects().Invoke(_rfcDestination);

        #endregion Methods
    }
}