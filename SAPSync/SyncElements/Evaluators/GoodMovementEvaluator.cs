using SAPSync.SyncElements.Validators;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Evaluators
{
    public class GoodMovementEvaluator : RecordEvaluator<GoodMovement, Tuple<long, int>>
    {
        public GoodMovementEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        protected override Tuple<long, int> GetIndexKey(GoodMovement record) => record.GetPrimaryKey();

        protected override IRecordValidator<GoodMovement> GetRecordValidator() => new GoodMovementValidator();    }
}
