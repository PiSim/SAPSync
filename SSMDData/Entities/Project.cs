using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public class Project
    {
        #region Constructors

        public Project()
        {
        }

        #endregion Constructors

        #region Properties

        public string Code { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }

        [Key]
        public int ID { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<WBSRelation> WBSDownRelations { get; set; }
        public virtual ICollection<WBSRelation> WBSLeftRelations { get; set; }
        public int WBSLevel { get; set; }

        public virtual ICollection<WBSRelation> WBSRelations { get; set; }
        public virtual ICollection<WBSRelation> WBSRightRelations { get; set; }
        public virtual ICollection<WBSRelation> WBSUpRelations { get; set; }

        #endregion Properties
    }
}