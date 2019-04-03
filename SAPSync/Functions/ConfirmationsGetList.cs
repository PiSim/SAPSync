namespace SAPSync.Functions
{
    public class ConfirmationsGetList : BAPIFunctionBase
    {
        #region Constructors

        public ConfirmationsGetList() : base()
        {
            _functionName = "BAPI_PROCORDCONF_GETLIST";
            _tableName = "confirmations";
        }

        #endregion Constructors

        #region Properties

        public int OrderNumber { get; set; } = 0;

        #endregion Properties

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
        }

        #endregion Methods
    }
}