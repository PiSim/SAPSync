using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public partial class WorkCenter
    {
        #region Properties

        [Key]
        public int ID { get; set; }

        public string ShortName { get; set; }

        #endregion Properties
    }
}