using SAPSync.Functions;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.SyncElements
{
    public class OrderEvaluator : RecordEvaluator<Order, int>
    {
        #region Methods

        protected override int GetIndexKey(Order record) => record.Number;

        #endregion Methods
    }

    public class SyncOrders : SyncSAPTable<Order>
    {
        #region Constructors

        public SyncOrders(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Ordini";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<Order> records)
        {
        }

        protected override IRecordEvaluator<Order> GetRecordEvaluator() => new OrderEvaluator();

        protected override IList<Order> ReadRecordTable() => new ReadOrders().Invoke(_rfcDestination);

        #endregion Methods
    }
}