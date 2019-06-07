using SAP.Middleware.Connector;
using SAPSync.SyncElements;
using SAPSync.SyncElements.ExcelWorkbooks;
using SSMD;
using System;
using System.Collections.Generic;
using System.IO;
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
                    syncElement.StartSync();
            }
            catch (Exception e)
            {
                throw new Exception("Sincronizzazione Fallita:" + e.Message, e);
            }
        }

        protected virtual void CreateLogEntry(string message)
        {
            string logPath = Properties.Settings.Default.LogFilePath;
            string logString = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + message;
            List<string> logLines = new List<string>
            {
                logString
            };

            File.AppendAllLines(logPath, logLines);
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            CreateLogEntry("Error: " + e.ErrorMessage.Replace('\n', ' '));
        }

        protected virtual void OnSyncFailureRaised(object sender, EventArgs e)
        {
            string logMessage = "Sincronizzazione fallita: " + (sender as ISyncElement).Name;
            CreateLogEntry(logMessage);
        }

        private void InitializeSyncElements()
        {
            try
            {
                RfcDestination rfcDestination = _reader.GetRfcDestination();
                _syncElements = new List<ISyncElement>
                {
                    new SyncWorkCenters(rfcDestination, _sSMDData),
                    new SyncMaterialFamilylevels(rfcDestination, _sSMDData),
                    new SyncMaterialFamilies(rfcDestination, _sSMDData),
                    new SyncProjects(rfcDestination, _sSMDData),
                    new SyncWBSRelations(rfcDestination, _sSMDData),
                    new SyncMaterials(rfcDestination, _sSMDData),
                    new SyncOrders(rfcDestination, _sSMDData),
                    new SyncOrderData(rfcDestination, _sSMDData),
                    new SyncRoutingOperations(rfcDestination, _sSMDData),
                    new SyncComponents(rfcDestination, _sSMDData),
                    new SyncOrderComponents(rfcDestination, _sSMDData),
                    new SyncConfirmations(rfcDestination, _sSMDData),
                    new SyncInspectionCharacteristics(rfcDestination, _sSMDData),
                    new SyncInspectionLots(rfcDestination, _sSMDData),
                    new SyncInspectionSpecifications(rfcDestination, _sSMDData),
                    new SyncInspectionPoints(rfcDestination, _sSMDData),
                    new SyncTrialMasterReport(_sSMDData),
                    new SyncTESTODPPROVA(_sSMDData)
                };

                foreach (ISyncElement syncElement in _syncElements)
                {
                    syncElement.SyncErrorRaised += OnSyncErrorRaised;
                    syncElement.SyncFailed += OnSyncFailureRaised;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione elementi di sincronizzazione fallita: " + e.Message);
            }
        }

        private void ResetAllProgress()
        {
            foreach (ISyncElement syncElement in SyncElements)
                syncElement.ResetProgress();
        }

        #endregion Methods
    }
}