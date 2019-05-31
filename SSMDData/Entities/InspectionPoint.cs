using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public partial class InspectionPoint
    {
        #region Constructors

        public InspectionPoint()
        {
        }

        #endregion Constructors

        #region Properties

        public double AvgValue { get; set; }
        public int CharNumber { get; set; }
        public DateTime InspectionDate { get; set; }

        public InspectionLot InspectionLot { get; set; }

        [ForeignKey("InspectionLot")]
        public long InspectionLotNumber { get; set; }

        public InspectionSpecification InspectionSpecification { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public int NodeNumber { get; set; }
        public int SampleNumber { get; set; }

        #endregion Properties

        #region Methods

        public Tuple<long, int, int, int> GetPrimaryKey() => new Tuple<long, int, int, int>(InspectionLotNumber, NodeNumber, CharNumber, SampleNumber);

        #endregion Methods
    }
}