using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            OrderData = new HashSet<OrderData>();
        }

        #endregion Constructors

        #region Properties

        public int ControlPlanNumber { get; set; }
        public int ID { get; set; }
        public virtual ICollection<InspectionLot> InspectionLots { get; set; }

        [Key]
        public int Number { get; set; }

        public virtual ICollection<OrderComponent> OrderComponents { get; set; }
        public virtual ICollection<OrderConfirmation> OrderConfirmations { get; set; }
        public virtual ICollection<OrderData> OrderData { get; set; }
        public string OrderType { get; set; }
        public long RoutingNumber { get; set; }
        public virtual ICollection<TestReport> TestReports { get; set; }
        public virtual ICollection<WorkPhaseLabData> WorkPhaseLabData { get; set; }

        public virtual ICollection<GoodMovement> GoodMovements { get; set; }

        #endregion Properties
    }
}