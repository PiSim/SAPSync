using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public interface IBAPIStructFunction
    {
        #region Methods

        IRfcStructure Invoke(RfcDestination rfcDestination);

        #endregion Methods
    }
}