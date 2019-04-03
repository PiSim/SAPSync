using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;

namespace SAPSync
{
    public class SyncInspectionPoints : SyncElement
    {
        #region Constructors

        public SyncInspectionPoints()
        {
            Name = "Punti di Controllo";
        }

        #endregion Constructors

        #region Methods

        public override void StartSync(RfcDestination destination, SSMDData sSMDData)
        {
            RetrieveInspectionPoints(destination);
        }

        private IRfcTable RetrieveInspectionPoints(RfcDestination destination)
        {
            IRfcTable output;

            try
            {
                output = new InspectionPointsGetList().Invoke(destination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionPoints error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}