using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public partial class MaterialFamily
    {
        #region Constructors

        public MaterialFamily()
        {
            Materials = new HashSet<Material>();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; set; }

        [Key]
        public int ID { get; set; }

        public string L1 { get; set; }
        public string L1Description { get; set; }
        public string L2 { get; set; }
        public string L2Description { get; set; }
        public string L3 { get; set; }
        public string L3Description { get; set; }

        public ICollection<Material> Materials { get; set; }

        #endregion Properties
    }
}