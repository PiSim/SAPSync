using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class Material
    {
        public Material()
        {
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public int? MaterialFamilyID { get; set; }

        public MaterialFamily MaterialFamily {get;set;}
        public ICollection<Order> Orders { get; set; }
    }
}
