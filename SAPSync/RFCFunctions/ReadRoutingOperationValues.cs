using SSMD;
using System.Globalization;

namespace SAPSync.RFCFunctions
{
    public class ReadRoutingOperationValues : ReadTableBase<RoutingOperation>
    {
        #region Constructors

        public ReadRoutingOperationValues()
        {
            _tableName = "AFVV";

            _fields = new string[]
            {
                "AUFPL",
                "APLZL",
                "BMSCH"
            };
        }

        #endregion Constructors

        #region Methods

        internal override RoutingOperation ConvertDataArray(string[] data)
        {
            if (!long.TryParse(data[0], out long routingOperationNumber)
                || !int.TryParse(data[1], out int routingOperationCounter)
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double baseQuantity))
                return null;

            RoutingOperation output = new RoutingOperation()
            {
                RoutingNumber = routingOperationNumber,
                RoutingCounter = routingOperationCounter,
                BaseQuantity = (int)baseQuantity
            };

            return output;
        }

        #endregion Methods
    }
}