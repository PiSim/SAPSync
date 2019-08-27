using SAP.Middleware.Connector;

namespace DMTAgent.SAP
{
    public class BAPITableFunctionBase
    {
        #region Fields

        internal string _functionName,
                _tableName;

        internal IRfcFunction _rfcFunction;

        #endregion Fields

        #region Methods

        public IRfcTable Invoke(RfcDestination rfcDestination)
        {
            InitializeFunction(rfcDestination);
            _rfcFunction.Invoke(rfcDestination);
            IRfcTable output = _rfcFunction.GetTable(_tableName);
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