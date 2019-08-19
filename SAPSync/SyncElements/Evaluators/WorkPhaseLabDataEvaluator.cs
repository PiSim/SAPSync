using SAPSync.SyncElements.Validators;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Evaluators
{
    public class WorkPhaseLabDataEvaluator : RecordEvaluator<WorkPhaseLabData, int>
    {
        public WorkPhaseLabDataEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }

        #region Methods

        protected override IRecordValidator<WorkPhaseLabData> GetRecordValidator() => new WorkPhaseLabDataValidator();
        
        protected override int GetIndexKey(WorkPhaseLabData record) => record.OrderNumber;

        #endregion Methods
    }
}
