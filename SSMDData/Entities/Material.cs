using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int ID { get; set; }

        public MaterialFamily MaterialFamily { get; set; }

        [ForeignKey("MaterialFamily")]
        public int? MaterialFamilyID { get; set; }

        public ICollection<Order> Orders { get; set; }

        #endregion Properties
    }
}