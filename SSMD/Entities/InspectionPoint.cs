using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class InspectionPoint
    {
        public InspectionPoint()
        {

        }

        public int Number { get; set; }
        public int InspectionOperationNumber { get; set; }
        public string UM { get; set; }
        public DateTime InspectionDate { get; set; }

        public InspectionOperation InspectionOperation { get; set; }
    }
}
