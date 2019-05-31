using System;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public class MaterialFamilyLevel
    {
        #region Constructors

        public MaterialFamilyLevel()
        {
        }

        #endregion Constructors

        #region Properties

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        [Key]
        public int ID { get; set; }

        public int Level { get; set; }

        #endregion Properties

        #region Methods

        public Tuple<int, string> GetIndexKey() => new Tuple<int, string>(Level, Code);

        #endregion Methods
    }
}