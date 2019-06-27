using SSMD;
using System.Globalization;

namespace SAPSync.Functions
{
    public class ReadOrderData : ReadTableBase<OrderData>
    {
        #region Constructors

        public ReadOrderData() : base()
        {
            _tableName = "AFKO";

            _fields = new string[]
            {
                "AUFNR",
                "AUFPL",
                "GAMNG",
                "PLNBEZ"
            };
        }

        #endregion Constructors

        #region Methods

        internal override OrderData ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int orderNumber)
                || !long.TryParse(data[1], out long routingNumber)
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double plannedQuantity))
                return null;

            OrderData output = new OrderData()
            {
                OrderNumber = orderNumber,
                RoutingNumber = routingNumber,
                PlannedQuantity = plannedQuantity,
                Material = new Material()
                {
                    Code = data[3]
                }
            };

            return output;
        }

        #endregion Methods
    }
}