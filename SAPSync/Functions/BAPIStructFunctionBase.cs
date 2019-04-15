using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public class BAPIStructFunctionBase : IBAPIStructFunction
    {
        #region Fields

        internal string _functionName,
                _structName;

        internal IRfcFunction _rfcFunction;

        #endregion Fields

        #region Constructors

        public BAPIStructFunctionBase()
        {
        }

        #endregion Constructors

        #region Methods

        public IRfcStructure Invoke(RfcDestination rfcDestination)
        {
            InitializeFunction(rfcDestination);
            _rfcFunction.Invoke(rfcDestination);
            IRfcStructure output = _rfcFunction.GetStructure(_structName);
            return output;
        }

        protected virtual void InitializeParameters()
        {
        }

        private void InitializeFunction(RfcDestination rfcDestination)
        {
            RfcRepository rfcRepository = rfcDestination.Repository;
            _rfcFunction = rfcRepository.CreateFunction(_functionName);
            InitializeParameters();
        }

        #endregion Methods
    }
}