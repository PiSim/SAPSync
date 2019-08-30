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

        public CreateMissingTrialLabData(IDataService<SSMDContext> dataService) : base()
        {
            SSMDData = dataService;
        }

        #region Methods

        public override void Start(ISubJob newJob)
        {
            base.Start(newJob);
            List<Order> trialRecords = SSMDData.RunQuery(new Query<Order, SSMDContext>()).Where(ord => ord.OrderType[0] == 'Z').ToList();
            IDictionary<int, WorkPhaseLabData> workPhaseDataIndex = SSMDData.RunQuery(new Query<WorkPhaseLabData, SSMDContext>()).ToDictionary(wpld => wpld.OrderNumber, wpld => wpld);

            List<WorkPhaseLabData> newTrials = new List<WorkPhaseLabData>();

            foreach (Order trial in trialRecords)
                if (!workPhaseDataIndex.ContainsKey(trial.Number))
                    newTrials.Add(new WorkPhaseLabData()
                    {
                        OrderNumber = trial.Number
                    });

            SSMDData.Execute(new InsertEntitiesCommand<SSMDContext>(newTrials));
        }

        private IDataService<SSMDContext> SSMDData {get;}

        #endregion Methods
    }
}