using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public interface IBAPITableFunction
    {
        #region Methods

        IRfcTable Invoke(RfcDestination rfcDestination);

        #endregion Methods
    }
}