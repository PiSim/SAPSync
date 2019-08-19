using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string FullCode => L1Code + L2Code + L3Code;

        [Key]
        public int ID { get; set; }

        public MaterialFamilyLevel L1 { get; set; }

        [NotMapped]
        public string L1Code => L1?.Code;

        [NotMapped]
        public string L1Description => L1?.Description;

        public int L1ID { get; set; }

        public MaterialFamilyLevel L2 { get; set; }

        [NotMapped]
        public string L2Code => L2?.Code;

        [NotMapped]
        public string L2Description => L2?.Description;

        public int L2ID { get; set; }

        public MaterialFamilyLevel L3 { get; set; }

        [NotMapped]
        public string L3Code => L3?.Code;

        [NotMapped]
        public string L3Description => L3?.Description;

        public int L3ID { get; set; }
        public virtual ICollection<Material> Materials { get; set; }

        #endregion Properties
    }
}