using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class WorkPhaseLabData
    {
        #region Properties

        public string Actions { get; set; }

        public string Analysis { get; set; }

        [Key]
        public int ID { get; set; }

        public string NotesC { get; set; }

        public string NotesG { get; set; }

        public string NotesP { get; set; }

        public string NotesS { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Order")]
        public int OrderNumber { get; set; }

        public string TrialScope { get; set; }

        #endregion Properties
    }
}