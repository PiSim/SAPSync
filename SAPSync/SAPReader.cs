using SAP.Middleware.Connector;
using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync
{
    public class SAPReader
    {
        #region Fields

        private string _destinationName = "PRD";
        private List<int> _inspectionLots;
        private RfcDestination _rfcDestination;
        private SSMDData _sSMDData;

        #endregion Fields

        #region Constructors

        public SAPReader()
        {
            TestConnection(_destinationName);
            _sSMDData = new SSMDData(new SSMDContextFactory());
        }

        #endregion Constructors

        #region Methods

        public ICollection<SyncElement> GetSyncElements()
        {
            List<SyncElement> elementList = new List<SyncElement>();

            elementList.Add(new SyncMaterials());
            elementList.Add(new SyncOrders());
            elementList.Add(new SyncConfirmations());
            elementList.Add(new SyncInspectionLots());
            elementList.Add(new SyncInspectionOperations());
            elementList.Add(new SyncInspectionPoints());

            return elementList;
        }

        public void RunSynchronization(SyncElement element)
        {
            if (element is INeedsInspectionLots)
                (element as INeedsInspectionLots).InspectionLots = _inspectionLots;

            element.StartSync(_rfcDestination ?? RfcDestinationManager.GetDestination(_destinationName),
                _sSMDData);
        }

        public bool TestConnection(string _destinationName)
        {
            bool result = false;

            try
            {
                _rfcDestination = RfcDestinationManager.GetDestination(_destinationName);
                if (_rfcDestination != null)
                {
                    _rfcDestination.Ping();
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