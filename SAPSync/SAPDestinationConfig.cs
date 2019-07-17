using SAP.Middleware.Connector;
using System.Configuration;

namespace SAPSync
{
    public class SAPDestinationConfig : IDestinationConfiguration
    {
        #region Events

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        #endregion Events

        #region Methods

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters prms = new RfcConfigParameters()
            {
                { RfcConfigParameters.Name, "PRD" },
                { RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"] },
                { RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"] },
                { RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_CLIENT"] },
                { RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"] },
                { RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"] },
                { RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"] },
                { RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"] },
                { RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"] }
            };

            return prms;
        }

        protected virtual void RaiseConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this.ToString(), new RfcConfigurationEventArgs(RfcConfigParameters.EventType.CHANGED));
        }

        #endregion Methods
    }
}