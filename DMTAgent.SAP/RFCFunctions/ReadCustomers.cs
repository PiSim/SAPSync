﻿using SSMD;

namespace DMTAgent.SAP
{
    public class ReadCustomers : ReadTableBase<Customer>
    {
        #region Constructors

        public ReadCustomers() : base()
        {
            _tableName = "KNA1";

            _fields = new string[]
            {
                "KUNNR",
                "NAME1",
                "NAME2"
            };
        }

        #endregion Constructors

        #region Methods

        internal override Customer ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int customerID))
                return null;

            Customer output = new Customer()
            {
                ID = customerID,
                Name = data[1],
                Name2 = data[2]
            };

            return output;
        }

        #endregion Methods
    }
}