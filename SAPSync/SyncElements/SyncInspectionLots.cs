using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;

namespace SAPSync
{
    public class SyncInspectionLots : SyncElement
    {
        #region Constructors

        public SyncInspectionLots()
        {
            Name = "Lotti di Controllo";
        }

        #endregion Constructors

        #region Methods

        public override void StartSync(RfcDestination rfcDestination, SSMDData sSMDData)
        {
            RetrieveInspectionLots(rfcDestination);
        }

        private IRfcTable RetrieveInspectionLots(RfcDestination rfcDestination)
        {
            IRfcTable output;

            try
            {
                output = new InspLotGetList().Invoke(rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionLots error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}