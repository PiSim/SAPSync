using System;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public class GoodMovement
    {
        #region Properties

        public Component Component { get; set; }

        public int ComponentID { get; set; }

        public long DocumentNumber { get; set; }

        public int ItemNumber { get; set; }
        public Order Order { get; set; }
        public int OrderNumber { get; set; }
        public double Quantity { get; set; }

        public string UM { get; set; }

        #endregion Properties

        #region Methods

        public Tuple<long, int> GetIndexKey() => new Tuple<long, int>(DocumentNumber, ItemNumber);

        #endregion Methods
    }
}