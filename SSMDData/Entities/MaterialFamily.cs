using System.Collections.Generic;

namespace SSMD
{
    public class MaterialFamily
    {
        #region Constructors

        public MaterialFamily()
        {
            Materials = new HashSet<Material>();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; set; }
        public int ID { get; set; }

        public ICollection<Material> Materials { get; set; }

        #endregion Properties
    }
}