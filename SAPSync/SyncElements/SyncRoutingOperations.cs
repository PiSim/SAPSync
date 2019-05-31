using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public class RoutingOperationEvaluator : RecordEvaluator<RoutingOperation, Tuple<long, int>>
    {
        protected override Tuple<long, int> GetIndexKey(RoutingOperation record) => new Tuple<long, int>(record.RoutingNumber, record.RoutingCounter);
    }


    public class SyncRoutingOperations : SyncElement<RoutingOperation>
    {
        public SyncRoutingOperations() : base()
        {
            Name = "Operazioni Ordine";
        }

        protected override IList<RoutingOperation> ReadRecordTable() => (new ReadRoutingOperations()).Invoke(_rfcDestination);

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new RoutingOperationEvaluator();
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new RecordValidator<RoutingOperation>();
        }
    }
}
