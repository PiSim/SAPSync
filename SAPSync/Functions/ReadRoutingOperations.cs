using SSMD;

namespace SAPSync.Functions
{
    public class ReadRoutingOperations : ReadTableBase<RoutingOperation>
    {
        #region Constructors

        public ReadRoutingOperations()
        {
            _tableName = "AFVV";

            _fields = new string[]
            {
                "AUFPL",
                "APLZL"
            };
        }

        #endregion Constructors

        #region Methods

        internal override RoutingOperation ConvertDataArray(string[] data)
        {
            if (!long.TryParse(data[0], out long routingOperationNumber)
                || !int.TryParse(data[1], out int routingOperationCounter))
                return null;

            RoutingOperation output = new RoutingOperation()
            {
                RoutingNumber = routingOperationNumber,
                RoutingCounter = routingOperationCounter
            };

            return output;
        }

        #endregion Methods
    }
}