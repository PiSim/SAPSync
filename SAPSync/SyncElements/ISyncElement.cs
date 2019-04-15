using SAP.Middleware.Connector;
using SSMD;
using System;
using System.ComponentModel;

namespace SAPSync.SyncElements
{
    public interface ISyncElement
    {
        #region Events

        event ProgressChangedEventHandler ProgressChanged;

        event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        string Name { get; }

        int PhaseProgress { get; }

        bool RequiresSync { get; set; }

        string SyncStatus { get; }

        #endregion Properties

        #region Methods

        void SetOnQueue();

        void StartSync(RfcDestination rfcDestination, SSMDData sSMDData);

        #endregion Methods
    }
}