using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class InspectionLot
    {
        public InspectionLot()
        {

        }

        public int LotNumber { get; set; }
        public int OrderNumber { get; set; }
        
        public Order Order { get; set; }
    }
}
