using System;
using System.Collections.Generic;
using System.Text;

namespace SSMD
{
    public class Order
    {
        public Order()
        {
            InspectionLots = new HashSet<InspectionLot>();
        }

        public int Number { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalScrap { get; set; }
        public int? InspectionLotNumber { get; set; }

        public ICollection<InspectionLot> InspectionLots { get; set; }
    }
}
