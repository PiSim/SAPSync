using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class RoutingOperation
    {
        public RoutingOperation()
        {

        }

        [Required]
        public long RoutingNumber { get; set; }

        [Required]
        public int RoutingCounter { get; set; }

        [ForeignKey("WorkCenter")]
        public int WorkCenterID { get; set; }

        public WorkCenter WorkCenter { get; set; }
    }
}
