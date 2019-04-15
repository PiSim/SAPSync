using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;

namespace SAPSync.Functions
{
    public class ConfirmationsGetList : BAPITableFunctionBase
    {
        #region Fields

        private IEnumerable<int> _orderNumberList;

        #endregion Fields

        #region Constructors

        public ConfirmationsGetList(IEnumerable<int> orderNumberList) : base()
        {
            _orderNumberList = orderNumberList;
            _functionName = "BAPI_PRODORDCONF_GETLIST";
            _tableName = "confirmations";
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();

            if (_orderNumberList == null)
                throw new ArgumentNullException("OrderNumberList");

            IRfcTable orderSelection = _rfcFunction.GetTable("order_range");

            foreach (int ord in _orderNumberList)
            {
                orderSelection.Append();
                orderSelection.SetValue("SIGN", "I");
                orderSelection.SetValue("OPTION", "EQ");
                orderSelection.SetValue("LOW", "00000" + ord.ToString());
            }
        }

        #endregion Methods
    }
}