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

        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<OrderComponent> OrderComponents { get; set; }

        #endregion Properties
    }
}