using SAP.Middleware.Connector;
using SSMD;
using System;

namespace SAPSync
{
    public abstract class SyncElement
    {
        #region Events

        private event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        public string Name { get; protected set; }
        public bool RequiresSync { get; set; } = true;
        public string Status { get; protected set; }

        #endregion Properties

        #region Methods

        public abstract void StartSync(RfcDestination rfcDestination, SSMDData sSMDData);

        protected virtual void OnStatusChanged(EventArgs e)
        {
            EventHandler handler = StatusChanged;
            handler?.Invoke(this, e);
        }

        private void ChangeStatus(string newStatus)
        {
            Status = newStatus;
            OnStatusChanged(new EventArgs());
        }

        #endregion Methods
    }
}