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
            RfcConfigParameters prms = new RfcConfigParameters();
            prms.Add(RfcConfigParameters.Name, "PRD");
            prms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"]);
            prms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"]);
            prms.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            prms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"]);
            prms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]);
            prms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            prms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]);
            prms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]);

            return prms;
        }

        #endregion Methods
    }
}