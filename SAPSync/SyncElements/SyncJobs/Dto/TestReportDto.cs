using OfficeOpenXml.Style;
using SSMD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.SyncJobs.Dto
{
    public class TestReportDto : DtoBase<TestReport>
    {
        public TestReportDto()
        {

        }

        public override void SetValues(TestReport testReport)
        {
            base.SetValues(testReport);
            MaterialCode = testReport.Order?.OrderData?.FirstOrDefault()?.Material?.Code;
            OrderType = testReport.Order?.OrderType;
            PlannedOrderQuantity = testReport.Order?.OrderData?.FirstOrDefault()?.PlannedQuantity;
            ColorName = testReport.Order?.OrderData?.FirstOrDefault().Material?.ColorComponent?.Description?.Replace("SKIN", "");
            Structure = testReport.Order?.OrderData?.FirstOrDefault().Material?.MaterialFamily?.L1?.Code;
            TrialScope = testReport.Order?.WorkPhaseLabData?.FirstOrDefault().TrialScope;
            PCA = testReport.Order?.OrderData?.FirstOrDefault().Material?.Project?.WBSRelations?.FirstOrDefault()?.Up?.WBSRelations?.FirstOrDefault()?.Up?.Code;
            Customer = testReport.Order?.OrderData?.FirstOrDefault().Material?.MaterialCustomer?.FirstOrDefault()?.Customer?.Name;
            ControlPlan = testReport.Order?.OrderData?.FirstOrDefault().Material?.ControlPlan;

            IEnumerable<InspectionPoint> thicknessPoints = testReport.Order?.InspectionLots?.SelectMany(inl => inl.InspectionPoints)?.Where(inl => Regex.IsMatch(inl.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOSPE"));
            IEnumerable<InspectionPoint> weightPoints = testReport.Order?.InspectionLots?.SelectMany(inl => inl.InspectionPoints)?.Where(inl => Regex.IsMatch(inl.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOPES"));

            EmbossingavgThickness = (thicknessPoints != null && thicknessPoints.Count() != 0) ? thicknessPoints.Average(inp => inp.AvgValue) : new double?();
            EmbossingAvgWeight = (weightPoints != null && weightPoints.Count() != 0) ? weightPoints.Average(inp => inp.AvgValue) : new double?();

        }

        protected Color GetColor(ExcelColor col)
        {
            string argb = col.Rgb;
            return Color.FromArgb(
                Convert.ToInt32(argb.Substring(0, 2), 16),
                Convert.ToInt32(argb.Substring(2, 2), 16),
                Convert.ToInt32(argb.Substring(4, 2), 16),
                Convert.ToInt32(argb.Substring(6, 2), 16)
                );
        }

        #region Properties

        [Column(27), Imported, Value, Exported]
        public double? BreakingElongationL { get; set; }

        [Column(27), FontColor, Exported]
        public Color BreakingElongationLFontColorEX { get; set; }

        [Column(27), FontColor, Imported]
        public ExcelColor BreakingElongationLFontColorIN { get; set; }

        public bool BreakingElongationLOK
        {
            get => BreakingElongationLFontColorIN?.Rgb == null ||
                GetColor(BreakingElongationLFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => BreakingElongationLFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(28), Imported, Value, Exported]
        public double? BreakingElongationT { get; set; }

        [Column(28), FontColor, Exported]
        public Color? BreakingElongationTFontColorEX { get; set; }

        [Column(28), FontColor, Imported]
        public ExcelColor BreakingElongationTFontColorIN { get; set; }

        public bool BreakingElongationTOK
        {
            get => BreakingElongationTFontColorIN?.Rgb == null ||
                GetColor(BreakingElongationTFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => BreakingElongationTFontColorEX = value ? Color.Black : Color.Red;
        }


        [Column(25), Imported, Value, Exported]
        public double? BreakingLoadL { get; set; }

        [Column(25), FontColor, Exported]
        public Color? BreakingLoadLFontColorEX { get; set; }

        [Column(25), FontColor, Imported]
        public ExcelColor BreakingLoadLFontColorIN { get; set; }

        public bool BreakingLoadLOK
        {
            get => BreakingLoadLFontColorIN?.Rgb == null ||
                GetColor(BreakingLoadLFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => BreakingLoadLFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(26), Imported, Value, Exported]
        public double? BreakingLoadT { get; set; }

        [Column(26), FontColor, Exported]
        public Color BreakingLoadTFontColorEX { get; set; }

        [Column(26), FontColor, Imported]
        public ExcelColor BreakingLoadTFontColorIN { get; set; }

        public bool BreakingLoadTOK
        {
            get => BreakingLoadTFontColorIN?.Rgb == null ||
                GetColor(BreakingLoadTFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => BreakingLoadTFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(19), Imported, Value, Exported]
        public string ColorJudgement { get; set; }

        [Column(19), FontColor, Exported]
        public Color ColorJudgementFontColorEX { get; set; }

        [Column(19), FontColor, Imported]
        public ExcelColor ColorJudgementFontColorIN { get; set; }

        public bool ColorJudgementOK
        {
            get => ColorJudgementFontColorIN?.Rgb == null ||
                GetColor(ColorJudgementFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => ColorJudgementFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(7), Value, Exported]
        public string ColorName { get; set; }

        [Column(12), Value, Exported]
        public int? ControlPlan { get; set; }

        [Column(13), Value, Exported]
        public string Customer { get; set; }

        [Column(23), Imported, Value, Exported]
        public double? DetachForceL { get; set; }

        [Column(23), FontColor, Exported]
        public Color DetachForceLFontColorEX { get; set; }

        [Column(23), FontColor, Imported]
        public ExcelColor DetachForceLFontColorIN { get; set; }

        public bool DetachForceLOK
        {
            get => DetachForceLFontColorIN?.Rgb == null ||
                GetColor(DetachForceLFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => DetachForceLFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(24), Imported, Value, Exported]
        public double? DetachForceT { get; set; }

        [Column(24), FontColor, Exported]
        public Color DetachForceTFontColorEX { get; set; }

        [Column(24), FontColor, Imported]
        public ExcelColor DetachForceTFontColorIN { get; set; }

        public bool DetachForceTOK
        {
            get => DetachForceTFontColorIN?.Rgb == null ||
                GetColor(DetachForceTFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => DetachForceTFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(16), Value, Exported]
        public double? EmbossingavgThickness { get; set; }

        [Column(15), Value, Exported]
        public double? EmbossingAvgWeight { get; set; }

        [Column(22), Imported, Value, Exported]
        public string FlammabilityEvaluation { get; set; }

        [Column(22), FontColor, Exported]
        public Color FlammabilityEvaluationFontColorEX { get; set; }

        [Column(22), FontColor, Imported]
        public ExcelColor FlammabilityEvaluationFontColorIN { get; set; }

        public bool FlammabilityEvaluationOK
        {
            get => FlammabilityEvaluationFontColorIN?.Rgb == null ||
                GetColor(FlammabilityEvaluationFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => FlammabilityEvaluationFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(20), Imported, Value, Exported]
        public double? Gloss { get; set; }

        [Column(20), FontColor, Exported]
        public Color GlossFontColorEX { get; set; }

        [Column(20), FontColor, Imported]
        public ExcelColor GlossFontColorIN { get; set; }

        public bool GlossOK
        {
            get => GlossFontColorIN?.Rgb == null ||
                GetColor(GlossFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => GlossFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(21), Imported, Value, Exported]
        public double? GlossZ { get; set; }

        [Column(21), FontColor, Exported]
        public Color GlossZFontColorEX { get; set; }

        [Column(21), FontColor, Imported]
        public ExcelColor GlossZFontColorIN { get; set; }

        public bool GlossZOK
        {
            get => GlossZFontColorIN?.Rgb == null ||
                GetColor(GlossZFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => GlossZFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(3), Value, Exported]
        public string MaterialCode { get; set; }

        [Column(10), Imported, Value, Exported]
        public string Notes { get; set; }

        [Column(14), Imported, Value, Exported]
        public string Notes2 { get; set; }

        [Column(1), Imported, Value, Exported]
        public int Number { get; set; }

        [Column(4), Imported, Value, Exported]
        public string Operator { get; set; }

        [Column(2), Imported, Value, Exported]
        public int? OrderNumber { get; set; }

        [Column(5), Value, Exported]
        public string OrderType { get; set; }

        [Column(33), Imported, Value, Exported]
        public string OtherTests { get; set; }

        [Column(33), FontColor, Exported]
        public Color OtherTestsFontColorEX { get; set; }

        [Column(33), FontColor, Imported]
        public ExcelColor OtherTestsFontColorIN { get; set; }

        public bool OtherTestsOK
        {
            get => OtherTestsFontColorIN?.Rgb == null ||
                GetColor(OtherTestsFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => OtherTestsFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(11), Value, Exported]
        public string PCA { get; set; }

        [Column(6), Value, Exported]
        public double? PlannedOrderQuantity { get; set; }

        [Column(31), Imported, Value, Exported]
        public double? SetL { get; set; }

        [Column(31), FontColor, Exported]
        public Color SetLFontColorEX { get; set; }

        [Column(31), FontColor, Imported]
        public ExcelColor SetLFontColorIN { get; set; }

        public bool SetLOK
        {
            get => SetLFontColorIN?.Rgb == null ||
                GetColor(SetLFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => SetLFontColorEX = (value) ? Color.Black : Color.Red;
        }

        [Column(32), Imported, Value, Exported]
        public double? SetT { get; set; }

        [Column(32), FontColor, Exported]
        public Color SetTFontColorEX { get; set; }

        [Column(32), FontColor, Imported]
        public ExcelColor SetTFontColorIN { get; set; }

        public bool SetTOK
        {
            get => SetTFontColorIN?.Rgb == null ||
                GetColor(SetTFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => SetTFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(29), Imported, Value, Exported]
        public double? StretchL { get; set; }

        [Column(29), FontColor, Exported]
        public Color StretchLFontColorEX { get; set; }

        [Column(29), FontColor, Imported]
        public ExcelColor StretchLFontColorIN { get; set; }

        public bool StretchLOK
        {
            get => StretchLFontColorIN?.Rgb == null ||
                GetColor(StretchLFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => StretchLFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(30), Imported, Value, Exported]
        public double? StretchT { get; set; }

        [Column(30), FontColor, Exported]
        public Color StretchTFontColorEX { get; set; }

        [Column(30), FontColor, Imported]
        public ExcelColor StretchTFontColorIN { get; set; }

        public bool StretchTOK
        {
            get => StretchTFontColorIN?.Rgb == null ||
                GetColor(StretchTFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => StretchTFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(8), Value, Exported]
        public string Structure { get; set; }

        [Column(18), Imported, Value, Exported]
        public double? Thickness { get; set; }

        [Column(18), FontColor, Exported]
        public Color ThicknessFontColorEX { get; set; }

        [Column(18), FontColor, Imported]
        public ExcelColor ThicknessFontColorIN { get; set; }

        public bool ThicknessOK
        {
            get => ThicknessFontColorIN?.Rgb == null ||
                GetColor(ThicknessFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => ThicknessFontColorEX = (value) ? Color.Black : Color.Red;
        }


        [Column(9), Value, Exported]
        public string TrialScope { get; set; }

        [Column(17), Imported, Value, Exported]
        public double? Weight { get; set; }

        [Column(17), FontColor, Exported]
        public Color WeightFontColorEX { get; set; }

        [Column(17), FontColor, Imported]
        public ExcelColor WeightFontColorIN { get; set; }

        public bool WeightOK
        {
            get => WeightFontColorIN?.Rgb == null ||
                GetColor(WeightFontColorIN).ToArgb() == Color.Black.ToArgb();
            set => WeightFontColorEX = (value) ? Color.Black : Color.Red;
        }


        #endregion Properties
    }
}
