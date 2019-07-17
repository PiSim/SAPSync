using SAP.Middleware.Connector;
using SAPSync.SyncElements;

namespace SAPSync.Functions
{
    public class BAPITableFunctionBase : SyncElementBase
    {
        #region Fields

        internal string _functionName,
                _tableName;

        internal IRfcFunction _rfcFunction;

        #endregion Fields

        #region Constructors

        public BAPITableFunctionBase()
        {
        }

        public override string Name => "BAPITableFunctionBase";

        #endregion Constructors

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