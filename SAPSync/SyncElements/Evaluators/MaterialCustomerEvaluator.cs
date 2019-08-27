using DataAccessCore;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;

namespace SAPSync.SyncElements.Evaluators
{
    public class MaterialCustomerEvaluator : RecordEvaluator<MaterialCustomer, Tuple<string, int>>
    {
        #region Constructors

        public MaterialCustomerEvaluator(RecordEvaluatorConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Query<MaterialCustomer, SSMDContext> GetIndexEntriesQuery() => new MaterialCustomersQuery() { EagerLoadingEnabled = true };

        protected override Tuple<string, int> GetIndexKey(MaterialCustomer record) => new Tuple<string, int>(record.Material?.Code, record.CustomerID);

        protected override IRecordValidator<MaterialCustomer> GetRecordValidator() => new MaterialCustomerValidator();

        #endregion Methods
    }
}