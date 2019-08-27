using SAP.Middleware.Connector;
using System;

namespace DMTAgent.SAP
{
    public class SAPReader
    {
        #region Fields

        private string _destinationName = "PRD";

        private bool destinationIsInitialized = false;
        #endregion Fields

        #region Constructors

        public SAPReader()
        {
        }

        #endregion Constructors

        #region Methods

        public RfcDestination GetRfcDestination()
        {
            InitSAP();
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

        public void InitSAP()
        {
            if (!destinationIsInitialized)
            {
                string destinationConfigName = "PRD";
                IDestinationConfiguration destinationConfig = null;

                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);

                bool destinationFound = false;

                try
                {
                    destinationFound = (RfcDestinationManager.GetDestination(destinationConfigName) != null);
                }
                catch
                {
                    destinationFound = false;
                }

                if (!destinationFound)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationIsInitialized = true;
                }
            }
        }
        #endregion Methods
    }
}