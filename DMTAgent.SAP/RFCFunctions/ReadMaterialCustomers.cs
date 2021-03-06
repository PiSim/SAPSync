﻿using SSMD;

namespace DMTAgent.SAP
{
    public class ReadMaterialCustomers : ReadTableBase<MaterialCustomer>
    {
        #region Constructors

        public ReadMaterialCustomers() : base()
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