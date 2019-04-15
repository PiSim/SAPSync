using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public partial class OrderConfirmation
    {
        #region Constructors

        public OrderConfirmation()
        {
        }

        #endregion Constructors

        #region Properties

        public int ConfirmationCounter { get; set; }
        public int ConfirmationNumber { get; set; }
        public bool DeletionFlag { get; set; }

        public DateTime EndTime { get; set; }
        public DateTime? EntryDate { get; set; }
        public Order Order { get; set; }
        public int OrderNumber { get; set; }

        [NotMapped]
        public object PrimaryKey => new object[] { ConfirmationNumber, ConfirmationCounter };

        public double Scrap { get; set; }
        public string ScrapCause { get; set; }
        public DateTime StartTime { get; set; }
        public string UM { get; set; }
        public string WIPIn { get; set; }
        public string WIPOut { get; set; }
        public int WorkCenterID { get; set; }
        public double Yield { get; set; }

        #endregion Properties
    }
}