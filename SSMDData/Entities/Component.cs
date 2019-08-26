using System.Collections.Generic;

namespace SSMD
{
    public partial class Component
    {
        #region Constructors

        public Component()
        {
            OrderComponents = new HashSet<OrderComponent>();
        }

        #endregion Constructors

        #region Properties

        public string Description { get; set; }
        public virtual ICollection<GoodMovement> GoodMovements { get; set; }
        public int ID { get; set; }

        public string Name { get; set; }
        public virtual ICollection<OrderComponent> OrderComponents { get; set; }

        #endregion Properties
    }
}