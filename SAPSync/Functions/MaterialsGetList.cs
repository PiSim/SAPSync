using SAP.Middleware.Connector;

namespace SAPSync.Functions
{
    public class MaterialsGetList : BAPIFunctionBase
    {
        #region Constructors

        public MaterialsGetList() : base()
        {
            _functionName = "BAPI_MATERIAL_GETLIST";
            _tableName = "matnrlist";
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("maxrows", 99999999);
            IRfcTable matSelection = _rfcFunction.GetTable("MATNRSELECTION");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("MATNR_LOW", "1*");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("MATNR_LOW", "2*");

            matSelection.Append();
            matSelection.SetValue("SIGN", "I");
            matSelection.SetValue("OPTION", "CP");
            matSelection.SetValue("MATNR_LOW", "3*");
        }

        #endregion Methods
    }
}