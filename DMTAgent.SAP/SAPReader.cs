using SAP.Middleware.Connector;
using System;

namespace DMTAgent.SAP
{
    // SP - Attempt to make reader stateless, initialization check completely delegated to Destination manager

    public class SAPReader
    {
        #region Fields

        private readonly string _destinationName = "PRD";

        #endregion Fields
        
        #region Methods

        public RfcDestination GetRfcDestination()
        {
            try
            {
                RfcDestination rfcDestination = RfcDestinationManager.GetDestination(_destinationName);
                TestConnection(rfcDestination);
                return rfcDestination;
            }
            catch (Exception e)
            {
                throw new Exception("DestinationConfig non inizializzata.", e);
            }
        }

        public void InitSAP()
        {
            try
            {
                IDestinationConfiguration destinationConfig = null;

                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(_destinationName);

                bool destinationFound = false;

                try
                {
                    destinationFound = (RfcDestinationManager.GetDestination(_destinationName) != null);
                }
                catch
                {
                    destinationFound = false;
                }

                if (!destinationFound)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore di inizializzazione RfcDestination", e);
            }
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