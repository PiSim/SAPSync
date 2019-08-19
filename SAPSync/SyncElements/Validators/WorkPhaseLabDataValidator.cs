using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Validators
{

    public class WorkPhaseLabDataValidator : IRecordValidator<WorkPhaseLabData>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public WorkPhaseLabData GetInsertableRecord(WorkPhaseLabData record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(rec => rec.Number, rec => rec);
        }

        public bool IsValid(WorkPhaseLabData record) => _orderIndex.ContainsKey(record.OrderNumber);

        #endregion Methods
    }
}
