using SSMD;
using System.Globalization;

namespace SAPSync.RFCFunctions
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
                "PLNBEZ",
                "IGMNG",
                "IASMG"
            };
        }

        #endregion Constructors

        #region Methods

        internal override OrderData ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int orderNumber)
                || !long.TryParse(data[1], out long routingNumber)
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double plannedQuantity)
                || !double.TryParse(data[4], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double totalYield)
                || !double.TryParse(data[5], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double totalScrap))
                return null;

            OrderData output = new OrderData()
            {
                OrderNumber = orderNumber,
                RoutingNumber = routingNumber,
                PlannedQuantity = plannedQuantity,
                TotalYield = totalYield,
                TotalScrap = totalScrap,
                Material = new Material()
                {
                    Code = data[3].Trim()
                }
            };

            return output;
        }

        #endregion Methods
    }
}