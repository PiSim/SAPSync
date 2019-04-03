namespace SAPSync.Functions
{
    public class InspectionPointsGetList : BAPIFunctionBase
    {
        #region Constructors

        public InspectionPointsGetList() : base()
        {
            _functionName = "BAPI_INSPOPER_GETDETAIL";
            _tableName = "inspoper_LIST";
        }

        #endregion Constructors

        #region Properties

        public int InspectionOperationNumber { get; set; } = 0;

        #endregion Properties

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("inspoper", InspectionOperationNumber);
        }

        #endregion Methods
    }
}