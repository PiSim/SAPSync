using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public partial class InspectionCharacteristic
    {
        #region Constructors

        public InspectionCharacteristic()
        {
            InspectionPoints = new HashSet<InspectionPoint>();
        }

        #endregion Constructors

        #region Properties

        public string Description { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual ICollection<InspectionPoint> InspectionPoints { get; set; }
        public double LowerSpecificationLimit { get; set; }

        [Required]
        public string Name { get; set; }

        public double TargetValue { get; set; }
        public string UM { get; set; }
        public double UpperSpecificationLimit { get; set; }

        #endregion Properties
    }
}