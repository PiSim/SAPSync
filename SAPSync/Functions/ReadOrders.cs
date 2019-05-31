using SSMD;

namespace SAPSync.Functions
{
    public class ReadOrders : ReadTableBase<Order>
    {
        #region Constructors

        public ReadOrders()
        {
            _tableName = "AUFK";

            _fields = new string[]
            {
                "AUFNR",
                "AUTYP",
                "MATNR"
            };
        }

        #endregion Constructors

        #region Methods

        internal override Order ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int orderNumber))
                return null;

            string orderType = data[1];
            string matCode = data[2];

            Order output = new Order()
            {
                Number = orderNumber,
                OrderType = orderType,
                Material = new Material()
                {
                    Code = matCode
                }
            };

            return output;
        }

        #endregion Methods
    }
}