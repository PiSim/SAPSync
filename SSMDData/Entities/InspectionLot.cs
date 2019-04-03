using System.Collections.Generic;

namespace SSMD
{
    public class InspectionLot
    {
        #region Constructors

        public InspectionLot()
        {
            InspectionOperations = new HashSet<InspectionOperation>();
        }

        #endregion Constructors

        #region Properties

        public int LotNumber { get; set; }
        public Order Order { get; set; }
        public int OrderNumber { get; set; }

        public ICollection<InspectionOperation> InspectionOperations { get; set; }

        #endregion Properties
    }
}