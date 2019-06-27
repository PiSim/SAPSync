using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class OrderData
    {
        #region Properties

        public bool HasSampleArrived { get; set; }
        public Material Material { get; set; }
        public int MaterialID { get; set; }
        public Order Order { get; set; }

        [Key, ForeignKey("Order")]
        public int OrderNumber { get; set; }

        public double PlannedQuantity { get; set; }
        public long RoutingNumber { get; set; }
        public virtual ICollection<RoutingOperation> RoutingOperations { get; set; }
        public DateTime? SampleArrivalDate { get; set; }
        public string SampleRollStatus { get; set; }

        #endregion Properties
    }
}