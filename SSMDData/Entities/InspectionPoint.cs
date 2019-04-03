using System;

namespace SSMD
{
    public class InspectionPoint
    {
        #region Constructors

        public InspectionPoint()
        {
        }

        #endregion Constructors

        #region Properties

        public DateTime InspectionDate { get; set; }
        public InspectionOperation InspectionOperation { get; set; }
        public int InspectionOperationNumber { get; set; }
        public int Number { get; set; }
        public string UM { get; set; }

        #endregion Properties
    }
}