using System;

namespace SSMD
{
    public class OrderConfirmation
    {
        #region Constructors

        public OrderConfirmation()
        {
        }

        #endregion Constructors

        #region Properties

        public int OrderNumber { get; set; }
        public int ConfirmationCounter { get; set; }
        public int ConfirmationNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public int InspectionCharacteristicNumber { get; set; }

        public Order Order { get; set; }

        #endregion Properties
    }
}