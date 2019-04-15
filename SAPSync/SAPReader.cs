using SAP.Middleware.Connector;
using System;

namespace SAPSync
{
    public class SAPReader
    {
        #region Fields

        private string _destinationName = "PRD";

        #endregion Fields

        #region Constructors

        public SAPReader()
        {
        }

        #endregion Constructors

        #region Methods

        public RfcDestination GetRfcDestination()
        {
            RfcDestination rfcDestination = RfcDestinationManager.GetDestination(_destinationName);
            TestConnection(rfcDestination);
            return rfcDestination;
        }

        public bool TestConnection(RfcDestination destination)
        {
            bool result = false;

            try
            {
                if (destination != null)
                {
                    destination.Ping();
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
                throw new Exception("Connection Failure: " + e.Message);
            }

            return result;
        }

        #endregion Methods
    }
}