using DataAccessCore;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent.SyncElements.Validators
{
    public class MaterialCustomerValidator : IRecordValidator<MaterialCustomer>
    {
        #region Fields

        private IDictionary<int, Customer> _customerIndex;
        private IDictionary<string, Material> _materialIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _customerIndex != null && _materialIndex != null;

        public MaterialCustomer GetInsertableRecord(MaterialCustomer record)
        {
            record.MaterialID = _materialIndex[record.Material.Code].ID;
            record.Material = null;

            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _materialIndex = sSMDData.RunQuery(new Query<Material, SSMDContext>()).ToDictionary(mat => mat.Code, mat => mat);
            _customerIndex = sSMDData.RunQuery(new Query<Customer, SSMDContext>()).ToDictionary(cus => cus.ID, cus => cus);
        }

        public bool IsValid(MaterialCustomer record) => record.Material != null
            && record.Material.Code != null
            && _materialIndex.ContainsKey(record.Material.Code)
            && _customerIndex.ContainsKey(record.CustomerID);

        #endregion Methods
    }
}