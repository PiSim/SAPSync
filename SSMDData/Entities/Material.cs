using System.Collections.Generic;

namespace SSMD
{
    public partial class Material
    {
        #region Constructors

        public Material()
        {
            Orders = new HashSet<Order>();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; set; }
        public int ID { get; set; }
        public MaterialFamily MaterialFamily { get; set; }
        public int? MaterialFamilyID { get; set; }
        public ICollection<Order> Orders { get; set; }

        #endregion Properties
    }
}