﻿using DMTAgent.SyncElements.Validators;
using SSMD;

namespace DMTAgent.SyncElements.Evaluators
{
    public class TestReportRecordEvaluator : RecordEvaluator<TestReport, int>
    {
        #region Constructors

        public TestReportRecordEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override int GetIndexKey(TestReport record) => record.Number;

        protected override IRecordValidator<TestReport> GetRecordValidator() => new TestReportRecordValidator();

        #endregion Methods
    }
}