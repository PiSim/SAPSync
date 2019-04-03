using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;

namespace SAPSync
{
    public class SyncOrders : SyncElement
    {
        #region Constructors

        public SyncOrders()
        {
            Name = "Ordini";
        }

        #endregion Constructors

        #region Methods

        public override void StartSync(RfcDestination destination, SSMDData sSMDData)
        {
            RetrieveOrders(destination);
        }

        private IRfcTable RetrieveOrders(RfcDestination destination)
        {
            IRfcTable output;

            try
            {
                output = new OrdersGetList().Invoke(destination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveOrders error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}