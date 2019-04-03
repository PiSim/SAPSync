using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;

namespace SAPSync
{
    public class SyncConfirmations : SyncElement
    {
        #region Constructors

        public SyncConfirmations()
        {
            Name = "Conferme";
        }

        #endregion Constructors

        #region Methods

        public override void StartSync(RfcDestination rfcDestination, SSMDData sSMDData)
        {
            RetrieveConfirmations(rfcDestination);
        }

        private IRfcTable RetrieveConfirmations(RfcDestination rfcDestination)
        {
            IRfcTable output;

            try
            {
                output = new ConfirmationsGetList().Invoke(rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveConfirmations error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}