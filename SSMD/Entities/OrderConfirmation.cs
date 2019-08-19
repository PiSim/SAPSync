using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class OrderConfirmation
    {
        public OrderConfirmation()
        {

        }

        public int ConfirmationNumber { get; set; }
        public int ConfirmationCounter { get; set; }
        public int InspectionCharacteristicNumber { get; set; }

        public DateTime EntryDate { get; set; }
    }
}
