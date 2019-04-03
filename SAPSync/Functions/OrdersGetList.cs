namespace SAPSync.Functions
{
    public class OrdersGetList : BAPIFunctionBase
    {
        #region Constructors

        public OrdersGetList() : base()
        {
            _functionName = "BAPI_PRODORD_GET_LIST";
            _tableName = "order_header";
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
        }

        #endregion Methods
    }
}