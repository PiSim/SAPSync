using System.Collections.Generic;

namespace SSMD
{
    public partial class Order
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

        public int ID { get; set; }
        public ICollection<InspectionLot> InspectionLots { get; set; }
        public Material Material { get; set; }
        public int MaterialID { get; set; }
        public int Number { get; set; }
        public ICollection<OrderComponent> OrderComponents { get; set; }
        public ICollection<OrderConfirmation> OrderConfirmations { get; set; }
        public string OrderType { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalScrap { get; set; }

        #endregion Properties
    }
}