using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public partial class InspectionLot
    {
        #region Constructors

        public InspectionLot()
        {
            InspectionPoints = new HashSet<InspectionPoint>();
        }

        #endregion Constructors

        #region Properties

        public ICollection<InspectionPoint> InspectionPoints { get; set; }

        [Key]
        public long Number { get; set; }

        public Order Order { get; set; }
        public int OrderNumber { get; set; }

        #endregion Properties
    }
}