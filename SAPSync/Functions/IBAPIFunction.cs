using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public interface IBAPIFunction
    {
        #region Methods

        IRfcTable Invoke(RfcDestination rfcDestination);

        #endregion Methods
    }
}