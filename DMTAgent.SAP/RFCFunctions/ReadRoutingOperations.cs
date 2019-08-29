using SSMD;

namespace DMTAgent.SAP
{
    public class ReadRoutingOperations : ReadTableBase<RoutingOperation>
    {
        #region Constructors

        public ReadRoutingOperations() : base()
        {
            _tableName = "AFVC";

            _fields = new string[]
            {
                "AUFPL",
                "APLZL",
                "ARBID"
            };
        }

        #endregion Constructors

        #region Methods

        internal override RoutingOperation ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int routingOperationNumber)
                || !int.TryParse(data[1], out int routingOperationCounter)
                || !int.TryParse(data[2], out int workCenterID))
                return null;

            RoutingOperation output = new RoutingOperation()
            {
                RoutingNumber = routingOperationNumber,
                RoutingCounter = routingOperationCounter,
                WorkCenterID = workCenterID
            };

            return output;
        }

        #endregion Methods
    }
}