using SAPSync.SyncElements.Validators;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Evaluators
{
    public class GoodMovementEvaluator : RecordEvaluator<GoodMovement, int>
    {
        protected override int GetIndexKey(GoodMovement record) => record.ID;

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new GoodMovementValidator();
        }
    }
}
