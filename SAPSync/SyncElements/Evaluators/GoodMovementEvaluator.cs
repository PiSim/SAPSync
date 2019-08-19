using SAPSync.SyncElements.Validators;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Evaluators
{
    public class GoodMovementEvaluator : RecordEvaluator<GoodMovement, Tuple<int,string>>
    {
        protected override Tuple<int, string> GetIndexKey(GoodMovement record) => new Tuple<int,string>(record.OrderNumber, record.Component?.Name);

        protected override IRecordValidator<GoodMovement> GetRecordValidator() => new GoodMovementValidator();
        
    }
}
