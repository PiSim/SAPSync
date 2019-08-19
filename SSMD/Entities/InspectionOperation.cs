using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class InspectionOperation
    {
        public InspectionOperation()
        {

        }

        public int Number { get; set; }
        public int InspectionLotNumber { get; set; }

        public InspectionLot InspectionLot { get; set; }
    }
}
