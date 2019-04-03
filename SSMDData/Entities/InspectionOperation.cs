using System.Collections.Generic;

namespace SSMD
{
    public class InspectionOperation
    {
        #region Constructors

        public InspectionOperation()
        {
            InspectionPoints = new HashSet<InspectionPoint>();
        }

        #endregion Constructors

        #region Properties

        public int InspectionCharacteristicID { get; set; }
        public InspectionLot InspectionLot { get; set; }
        public int InspectionLotNumber { get; set; }
        public int Number { get; set; }

        public InspectionCharacteristic InspectionCharacteristic {get;set; }
        public ICollection<InspectionPoint> InspectionPoints { get; set; }

        #endregion Properties
    }
}