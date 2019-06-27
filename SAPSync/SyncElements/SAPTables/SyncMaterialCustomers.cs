using SAPSync.Functions;
using SAPSync.SyncElements.Evaluators;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements.SAPTables
{
    public class SyncMaterialCustomers : SyncSAPTable<MaterialCustomer>
    {
        #region Constructors

        public SyncMaterialCustomers(SyncElementConfiguration elementConfiguration) : base(elementConfiguration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Clienti Materiale";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<MaterialCustomer> records)
        {
        }

        protected override IRecordEvaluator<MaterialCustomer> GetRecordEvaluator() => new MaterialCustomerEvaluator();

        protected override IList<MaterialCustomer> ReadRecordTable() => new ReadMaterialCustomers().Invoke(_rfcDestination);

        #endregion Methods
    }
}