using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class OrderComponent
    {
        public OrderComponent()
        {

        }

        public int OrderNumber { get; set; }
        public int ComponentID { get; set; }

        public Order Order { get; set; }
        public Component Component { get; set; }
    }
}
