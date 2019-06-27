using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class TestReport
    {
        #region Properties

        public double? BreakingElongationL { get; set; }

        public double? BreakingElongationT { get; set; }

        public double? BreakingLoadL { get; set; }

        public double? BreakingLoadT { get; set; }

        public string ColorJudgement { get; set; }

        public double? DetachForceL { get; set; }

        public double? DetachForceT { get; set; }

        public string FlammabilityEvaluation { get; set; }

        public double? Gloss { get; set; }

        public double? GlossZ { get; set; }

        public string Notes { get; set; }
        public string Notes2 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        public string Operator { get; set; }

        public Order Order { get; set; }
        public int? OrderNumber { get; set; }

        public string OtherTests { get; set; }
        public double? SetL { get; set; }
        public double? SetT { get; set; }
        public double? StretchL { get; set; }
        public double? StretchT { get; set; }
        public double? Thickness { get; set; }
        public double? Weight { get; set; }

        #endregion Properties
    }
}