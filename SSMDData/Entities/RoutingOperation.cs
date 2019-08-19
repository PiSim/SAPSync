using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class RoutingOperation
    {
        #region Constructors

        public RoutingOperation()
        {
        }

        #endregion Constructors

        #region Properties

        public int BaseQuantity { get; set; } = 0;
        public OrderData OrderData { get; set; }

        [Required]
        public int RoutingCounter { get; set; }

        [Required]
        public long RoutingNumber { get; set; }

        public WorkCenter WorkCenter { get; set; }

        [ForeignKey("WorkCenter")]
        public int WorkCenterID { get; set; }

        #endregion Properties
    }
}