using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.SAPTables
{
    public class CustomerEvaluator : RecordEvaluator<Customer, int>
    {
        #region Methods

        protected override int GetIndexKey(Customer record) => record.ID;

        #endregion Methods
    }

    public class SyncCustomers : SyncSAPTable<Customer>
    {
        #region Constructors

        public SyncCustomers(SyncElementConfiguration elementConfiguration) : base(elementConfiguration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Clienti";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<Customer> records)
        {
        }

        protected override IRecordEvaluator<Customer> GetRecordEvaluator() => new CustomerEvaluator();

        protected override IList<Customer> ReadRecordTable() => new ReadCustomers().Invoke(_rfcDestination);

        #endregion Methods
    }
}