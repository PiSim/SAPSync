namespace SAPSync.Functions
{
    public class ConfirmationGetDetail : BAPIStructFunctionBase
    {
        #region Fields

        private int _confirmationNumber, _confirmationCounter;

        #endregion Fields

        #region Constructors

        public ConfirmationGetDetail(int confirmationNumber, int confirmationCounter)
        {
            _functionName = "BAPI_PRODORDCONF_GETDETAIL";
            _structName = "conf_detail";
            _confirmationNumber = confirmationNumber;
            _confirmationCounter = confirmationCounter;
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("confirmation", _confirmationNumber);
            _rfcFunction.SetValue("confirmationcounter", _confirmationCounter);
        }

        #endregion Methods
    }
}