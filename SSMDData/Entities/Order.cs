using System.Collections.Generic;

namespace SSMD
{
    public class Order
    {
        #region Constructors

        public Order()
        {
            InspectionLots = new HashSet<InspectionLot>();
            OrderComponents = new HashSet<OrderComponent>();
            OrderConfirmations = new HashSet<OrderConfirmation>();

        }

        #endregion Constructors

        #region Properties

        public int? InspectionLotNumber { get; set; }
        public ICollection<InspectionLot> InspectionLots { get; set; }
        public int Number { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalScrap { get; set; }
        public int MaterialID { get; set; }

        public Material Material { get; set; }

        public ICollection<OrderConfirmation> OrderConfirmations { get; set; }
        public ICollection<OrderComponent> OrderComponents { get; set; }

        #endregion Properties
    }
}