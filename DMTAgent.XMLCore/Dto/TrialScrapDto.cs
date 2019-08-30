using SSMD;
using System;
using System.Linq;

namespace DMTAgent.XMLCore
{
    public class TrialScrapDto : DtoBase<IGrouping<Tuple<Order, string>, OrderConfirmation>>
    {
        #region Properties

        [Column(10), Value, Exported]
        public double CauseScrapPercentage => (TotalProduced == 0) ? 0 : Scrap / TotalProduced;

        [Column(8), Value, Exported]
        public string CauseShortText { get; set; }

        [Column(4), Value, Exported]
        public string Color { get; set; }

        [Column(6), Value, Exported]
        public DateTime EndDate { get; set; }

        [Column(5), Value, Exported]
        public string FamilyCode { get; set; }

        [Column(3), Value, Exported]
        public string MaterialCode { get; set; }

        [Column(1), Value, Exported]
        public int OrderNumber { get; set; }

        [Column(2), Value, Exported]
        public string OrderType { get; set; }

        [Column(9), Value, Exported]
        public double Scrap { get; set; }

        [Column(7), Value, Exported]
        public double TotalProduced { get; set; } = 0;

        [Column(11), Value, Exported]
        public double TotalScrap { get; set; }

        [Column(12), Value, Exported]
        public double TotalScrapPercentage => (TotalProduced == 0) ? 0 : TotalScrap / TotalProduced;

        #endregion Properties

        #region Methods

        public override void SetValues(IGrouping<Tuple<Order, string>, OrderConfirmation> entity)
        {
            OrderNumber = entity.Key.Item1.Number;
            OrderType = entity.Key.Item1.OrderType;
            TotalScrap = (entity.Key.Item1.OrderData?.FirstOrDefault()?.TotalScrap == null) ? 0 : entity.Key.Item1.OrderData.First().TotalScrap;
            TotalProduced = TotalScrap + ((entity.Key.Item1.OrderData?.FirstOrDefault()?.TotalYield == null) ? 0 : entity.Key.Item1.OrderData.First().TotalYield);
            Scrap = entity.Sum(con => con.Scrap);
            MaterialCode = entity.Key.Item1.OrderData?.FirstOrDefault()?.Material?.Code;
            Color = entity.Key.Item1.OrderData?.FirstOrDefault()?.Material?.ColorComponent?.Description;
            FamilyCode = entity.Key.Item1.OrderData?.FirstOrDefault()?.Material?.MaterialFamily?.L1?.Code;
            CauseShortText = entity.Key.Item2;
            EndDate = entity.Max(con => con.EndTime);
        }

        #endregion Methods
    }
}