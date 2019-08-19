using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public partial class ScrapCause
    {
        #region Constructors

        public ScrapCause()
        {
        }

        #endregion Constructors

        #region Properties

        public string Code { get; set; }
        public string Description { get; set; }

        [Key]
        public int ID { get; set; }

        #endregion Properties
    }
}