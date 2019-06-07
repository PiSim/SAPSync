using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class OrderData
    {
        #region Constructors

        public OrderData()
        {
            RoutingOperations = new HashSet<RoutingOperation>();
        }

        #endregion Constructors

        #region Properties

        public Order Order { get; set; }

        [Key, ForeignKey("Order")]
        public int OrderNumber { get; set; }

        public double PlannedQuantity { get; set; }
        public long RoutingNumber { get; set; }
        public virtual ICollection<RoutingOperation> RoutingOperations { get; set; }

        #endregion Properties
    }
}