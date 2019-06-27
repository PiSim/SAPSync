using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.Validators
{
    public class GoodMovementValidator : IRecordValidator<GoodMovement>
    {
        private IDictionary<int, Order> _orderIndex;
        private IDictionary<string, Material> _materialIndex;

        public bool CheckIndexesInitialized() => _orderIndex != null && _materialIndex != null;

        public GoodMovement GetInsertableRecord(GoodMovement record)
        {
            record.MaterialID = _materialIndex[record.Material.Code].ID;
            record.Material = null;
            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number, ord => ord);
            _materialIndex = sSMDData.RunQuery(new Query<Material, SSMDContext>()).ToDictionary(mat => mat.Code, mat => mat);
        }

        public bool IsValid(GoodMovement record) => _orderIndex.ContainsKey(record.OrderNumber)
            && record.Material != null
            && _materialIndex.ContainsKey(record.Material.Code);
    }
}
