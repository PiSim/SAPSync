using SSMD;

namespace SAPSync.RFCFunctions
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
                "AUART"
            };
        }

        #endregion Constructors

        #region Methods

        internal override Order ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int orderNumber))
                return null;

            string orderType = data[1];

            Order output = new Order()
            {
                Number = orderNumber,
                OrderType = orderType,
            };

            return output;
        }

        #endregion Methods
    }
}