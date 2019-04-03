namespace SAPSync.Functions
{
    public class InspectionOperationsGetList : BAPIFunctionBase
    {
        #region Constructors

        public InspectionOperationsGetList() : base()
        {
            _functionName = "BAPI_INSPOPER_GETLIST";
            _tableName = "inspoper_LIST";
        }

        #endregion Constructors

        #region Properties

        public int InspectionLotNumber { get; set; } = 0;

        #endregion Properties

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("insplot", InspectionLotNumber);
        }

        #endregion Methods
    }
}