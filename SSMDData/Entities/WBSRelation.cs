using System;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public class WBSRelation
    {
        #region Properties

        public Project Down { get; set; }

        public Nullable<int> DownID { get; set; }

        [Key]
        public int ID { get; set; }

        public Project Left { get; set; }

        public Nullable<int> LeftID { get; set; }

        public Project Project { get; set; }
        public int ProjectID { get; set; }

        public Project Right { get; set; }
        public Nullable<int> RightID { get; set; }

        public Project Up { get; set; }
        public Nullable<int> UpID { get; set; }

        #endregion Properties
    }
}