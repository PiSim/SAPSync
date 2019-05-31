using SAPSync.SyncElements;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncManager
    {
        #region Fields

        private readonly SSMDData _sSMDData;
        private SAPReader _reader;
        private List<ISyncElement> _syncElements;

        #endregion Fields

        #region Constructors

        public SyncManager()
        {
            _reader = new SAPReader();
            _sSMDData = new SSMDData(new SSMDContextFactory());
        }

        #endregion Constructors

        #region Properties

        public ICollection<ISyncElement> SyncElements
        {
            get
            {
                if (_syncElements == null)
                    InitializeSyncElements();

                return _syncElements;
            }
        }

        #endregion Properties

        #region Methods

        public void StartSync()
        {
            try
            {
                ResetAllProgress();
                IEnumerable<ISyncElement> toSync = SyncElements.Where(syel => syel.RequiresSync);

                foreach (ISyncElement syncElement in toSync)
                    syncElement.SetOnQueue();

                foreach (ISyncElement syncElement in toSync)
                    syncElement.StartSync(_reader.GetRfcDestination(), _sSMDData);

            }
            catch (Exception e)
            {
                throw new Exception("Sincronizzazione Fallita:" + e.Message, e);
            }
        }

        private void ResetAllProgress()
        {
            foreach (ISyncElement syncElement in SyncElements)
                syncElement.ResetProgress();
        }

        private void InitializeSyncElements()
        {
            try
            {
                _syncElements = new List<ISyncElement>();
                _syncElements.Add(new SyncWorkCenters());
                _syncElements.Add(new SyncMaterialFamilylevels());
                _syncElements.Add(new SyncMaterialFamilies());
                _syncElements.Add(new SyncMaterials());
                _syncElements.Add(new SyncOrders());
                _syncElements.Add(new SyncRoutingOperations());
                _syncElements.Add(new SyncComponents());
                _syncElements.Add(new SyncOrderComponents());
                _syncElements.Add(new SyncConfirmations());
                _syncElements.Add(new SyncInspectionCharacteristics());
                _syncElements.Add(new SyncInspectionLots());
                _syncElements.Add(new SyncInspectionSpecifications());
                _syncElements.Add(new SyncInspectionPoints());
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione elementi di sincronizzazione fallita: " + e.Message);
            }
        }

        #endregion Methods
    }
}