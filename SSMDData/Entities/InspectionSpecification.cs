using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class InspectionSpecification
    {
        #region Constructors

        public InspectionSpecification()
        {
            InspectionPoints = new HashSet<InspectionPoint>();
        }

        #endregion Constructors

        #region Properties

        public int CharacteristicNumber { get; set; }

        public InspectionCharacteristic InspectionCharacteristic { get; set; }

        [ForeignKey("InspectionCharacteristic")]
        public int InspectionCharacteristicID { get; set; }

        public InspectionLot InspectionLot { get; set; }

        [ForeignKey("InspectionLot")]
        public long InspectionLotNumber { get; set; }

        public ICollection<InspectionPoint> InspectionPoints { get; set; }
        public double LowerSpecificationLimit { get; set; }
        public int NodeNumber { get; set; }
        public double TargetValue { get; set; }
        public string UM { get; set; }
        public double UpperSpecificationLimit { get; set; }

        #endregion Properties

        #region Methods

        public Tuple<long, int, int> GetPrimaryKey() => new Tuple<long, int, int>(InspectionLotNumber, NodeNumber, CharacteristicNumber);

        #endregion Methods
    }
}