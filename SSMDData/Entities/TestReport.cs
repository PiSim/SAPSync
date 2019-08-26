using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSMD
{
    public class TestReport
    {
        #region Properties

        public double? BreakingElongationL { get; set; }

        public bool BreakingElongationLOK { get; set; }
        public double? BreakingElongationT { get; set; }

        public bool BreakingElongationTOK { get; set; }
        public double? BreakingLoadL { get; set; }

        public bool BreakingLoadLOK { get; set; }
        public double? BreakingLoadT { get; set; }

        public bool BreakingLoadTOK { get; set; }
        public string ColorJudgement { get; set; }

        public bool ColorJudgementOK { get; set; }
        public double? DetachForceL { get; set; }

        public bool DetachForceLOK { get; set; }
        public double? DetachForceT { get; set; }

        public bool DetachForceTOK { get; set; }
        public string FlammabilityEvaluation { get; set; }
        public bool FlammabilityEvaluationOK { get; set; }

        public double? Gloss { get; set; }

        public bool GlossOK { get; set; }
        public double? GlossZ { get; set; }
        public bool GlossZOK { get; set; }
        public string Notes { get; set; }
        public string Notes2 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        public string Operator { get; set; }

        public Order Order { get; set; }
        public int? OrderNumber { get; set; }

        public string OtherTests { get; set; }
        public bool OtherTestsOK { get; set; }
        public double? SetL { get; set; }
        public bool SetLOK { get; set; }
        public double? SetT { get; set; }
        public bool SetTOK { get; set; }
        public double? StretchL { get; set; }
        public bool StretchLOK { get; set; }
        public double? StretchT { get; set; }
        public bool StretchTOK { get; set; }
        public double? Thickness { get; set; }
        public bool ThicknessOK { get; set; }
        public double? Weight { get; set; }
        public bool WeightOK { get; set; }

        #endregion Properties
    }
}