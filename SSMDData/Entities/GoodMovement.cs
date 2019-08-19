using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMD
{
    public class GoodMovement
    {
        [Key]
        public int ID { get; set; }

        public int ComponentID { get; set; }

        public Component Component { get; set; }

        
        public int OrderNumber { get; set; }

        public Order Order { get; set; }

        public double Quantity { get; set; }

        public string UM { get; set; }

        public long DocumentNumber { get; set; }

        public int ItemNumber { get; set; }

        public Tuple<long, int> GetPrimaryKey() => new Tuple<long, int>(DocumentNumber, ItemNumber);
    }
}
