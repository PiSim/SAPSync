using System;
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

        [ForeignKey("ColorComponent")]
        public int? ColorComponentID { get; set; }

        public Component ColorComponent { get; set; }

        public int ControlPlan { get; set; }

        [Key]
        public int ID { get; set; }

        public MaterialFamily MaterialFamily { get; set; }

        [ForeignKey("MaterialFamily")]
        public int? MaterialFamilyID { get; set; }

        public ICollection<Order> Orders { get; set; }
        public Project Project { get; set; }
        public Nullable<int> ProjectID { get; set; }

        #endregion Properties
    }
}