using SAPSync.SyncElements.Validators;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Evaluators
{
    public class TestReportRecordEvaluator : RecordEvaluator<TestReport, int>
    {
        public TestReportRecordEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<TestReport> GetRecordValidator() => new TestReportRecordValidator();

        protected override int GetIndexKey(TestReport record) => record.Number;

        #endregion Methods
    }
}
