using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync
{
    public class SyncInspectionOperations : SyncElement, INeedsInspectionLots
    {
        #region Constructors

        public SyncInspectionOperations()
        {
            Name = "Operazioni di Controllo";
        }

        #endregion Constructors

        #region Properties

        public ICollection<int> InspectionLots { get; set; }

        #endregion Properties

        #region Methods

        public override void StartSync(RfcDestination rfcDestination, SSMDData sSMDData)
        {
            RetrieveInspectionOperationss(rfcDestination);
        }

        private List<IRfcTable> RetrieveInspectionOperationss(RfcDestination rfcDestination)
        {
            List<IRfcTable> output = new List<IRfcTable>();

            if (InspectionLots == null)
                throw new ArgumentNullException("LotNumbers");

            try
            {
                IRfcTable currentTable;
                foreach (int lotNumber in InspectionLots)
                {
                    currentTable = new InspectionOperationsGetList() { InspectionLotNumber = lotNumber }.Invoke(rfcDestination);
                    output.Add(currentTable);
                }
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionOperations error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}