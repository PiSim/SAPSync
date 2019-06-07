using System;
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

        public bool HasRollArrived { get; set; }

        public string Notes { get; set; }

        [Key]
        public int Number { get; set; }

        public string Operator { get; set; }

        [ForeignKey("Order")]
        public int? OrderNumber { get; set; }

        public string OtherTests { get; set; }
        public string RollStatus { get; set; }
        public double? SetL { get; set; }
        public double? SetT { get; set; }
        public double? StretchL { get; set; }
        public double? StretchT { get; set; }
        public double? Thickness { get; set; }
        public double? Weight { get; set; }
        private DateTime RollArrivalDate { get; set; }

        #endregion Properties
    }
}