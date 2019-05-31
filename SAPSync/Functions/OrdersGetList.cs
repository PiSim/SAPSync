using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public class OrdersGetList : BAPITableFunctionBase
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
            IRfcTable orderSelection = _rfcFunction.GetTable("order_number_range");
            orderSelection.Append();
            orderSelection.SetValue("SIGN", "I");
            orderSelection.SetValue("OPTION", "CP");
            orderSelection.SetValue("LOW", "1000000");
            orderSelection.SetValue("HIGH", "2999999");

            IRfcTable matSelection = _rfcFunction.GetTable("material_range");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("LOW", "1*");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("LOW", "2*");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("LOW", "3*");
        }

        #endregion Methods
    }
}