using SSMD;

namespace SAPSync.Functions
{
    public class ReadMaterialCustomers : ReadTableBase<MaterialCustomer>
    {
        public override string Name => "ReadMaterialCustomers";
        #region Constructors

        public ReadMaterialCustomers()
        {
            _tableName = "KNMT";
            _fields = new string[]
            {
                "MATNR",
                "KUNNR"
            };
        }

        #endregion Constructors

        #region Methods

        internal override MaterialCustomer ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[1], out int customerNumber))
                return null;

            MaterialCustomer output = new MaterialCustomer()
            {
                Material = new Material()
                {
                    Code = data[0].Trim()
                },
                CustomerID = customerNumber
            };

            return output;
        }

        #endregion Methods
    }
}