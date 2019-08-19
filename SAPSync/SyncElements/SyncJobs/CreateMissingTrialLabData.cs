using DataAccessCore;
using DataAccessCore.Commands;
using Microsoft.EntityFrameworkCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements.ExcelWorkbooks
{
    public class CreateMissingTrialLabData : SyncJobBase
    {

        public override string Name => "Creazione Note Prova mancanti";
        
        #region Methods
        
        protected override void Execute()
        {
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

        #endregion Methods
    }
}