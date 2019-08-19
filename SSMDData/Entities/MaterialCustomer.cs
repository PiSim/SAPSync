using System;

namespace SSMD
{
    public class MaterialCustomer
    {
        #region Properties

        public Customer Customer { get; set; }
        public int CustomerID { get; set; }

        public Material Material { get; set; }
        public int MaterialID { get; set; }

        #endregion Properties

        #region Methods

        public Tuple<int, int> GetPrimaryKey() => new Tuple<int, int>(MaterialID, CustomerID);

        #endregion Methods
    }
}