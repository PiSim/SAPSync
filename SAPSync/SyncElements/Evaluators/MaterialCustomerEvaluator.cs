using DataAccessCore;
using SAPSync.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;

namespace SAPSync.SyncElements.Evaluators
{
    public class MaterialCustomerEvaluator : RecordEvaluator<MaterialCustomer, Tuple<string, int>>
    {
        #region Methods

        protected override IRecordValidator<MaterialCustomer> GetRecordValidator() => new MaterialCustomerValidator();

        protected override Query<MaterialCustomer, SSMDContext> GetIndexEntriesQuery() => new MaterialCustomersQuery() { EagerLoadingEnabled = true };

        protected override Tuple<string, int> GetIndexKey(MaterialCustomer record) => new Tuple<string, int>(record.Material?.Code, record.CustomerID);

        #endregion Methods
    }
}