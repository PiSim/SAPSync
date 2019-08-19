using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class Component
    {
        public Component()
        {
            OrderComponents = new HashSet<OrderComponent>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<OrderComponent> OrderComponents { get; set; }
    }
}
