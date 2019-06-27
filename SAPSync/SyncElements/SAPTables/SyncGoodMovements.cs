using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Functions;
using SAPSync.SyncElements.Evaluators;

namespace SAPSync.SyncElements.SAPTables
{
    public class SyncGoodMovements : SyncSAPTable<GoodMovement>
    {
        public SyncGoodMovements(SyncElementConfiguration elementConfiguration) : base(elementConfiguration)
        {

        }

        protected override void ExecuteExport(IEnumerable<GoodMovement> records)
        {

        }

        protected override IRecordEvaluator<GoodMovement> GetRecordEvaluator() => new GoodMovementEvaluator();

        protected override IList<GoodMovement> ReadRecordTable() => new ReadGoodMovements().Invoke(_rfcDestination);
    }
}
