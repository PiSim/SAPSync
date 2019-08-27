using DataAccessCore;
using DataAccessCore.Commands;
using DMTAgent.Infrastructure;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent.SyncElements.ExcelWorkbooks
{
    public class CreateMissingTrialLabData : SyncOperationBase
    {
        #region Properties

        public override string Name => "Creazione Note Prova mancanti";

        #endregion Properties

        #region Methods

        public override void Start(ISubJob newJob)
        {
            base.Start(newJob);
            List<Order> trialRecords = GetSSMDData().RunQuery(new Query<Order, SSMDContext>()).Where(ord => ord.OrderType[0] == 'Z').ToList();
            IDictionary<int, WorkPhaseLabData> workPhaseDataIndex = GetSSMDData().RunQuery(new Query<WorkPhaseLabData, SSMDContext>()).ToDictionary(wpld => wpld.OrderNumber, wpld => wpld);

            List<WorkPhaseLabData> newTrials = new List<WorkPhaseLabData>();

            foreach (Order trial in trialRecords)
                if (!workPhaseDataIndex.ContainsKey(trial.Number))
                    newTrials.Add(new WorkPhaseLabData()
                    {
                        OrderNumber = trial.Number
                    });

            GetSSMDData().Execute(new InsertEntitiesCommand<SSMDContext>(newTrials));
        }

        private SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        #endregion Methods
    }
}