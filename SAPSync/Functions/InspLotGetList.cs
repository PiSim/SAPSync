namespace SAPSync.Functions
{
    public class InspLotGetList : BAPIFunctionBase
    {
        #region Constructors

        public InspLotGetList() : base()
        {
            _functionName = "BAPI_INSPLOT_GETLIST";
            _tableName = "insplot_LIST";
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("max_rows", 99999999);
        }

        #endregion Methods
    }
}