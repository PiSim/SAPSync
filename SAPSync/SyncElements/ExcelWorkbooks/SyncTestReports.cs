using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAPSync.SyncElements.ExcelWorkbooks
{
    public class SyncTestReports : SyncXmlReport<TestReport, TestReportDto>
    {
        #region Constructors

        public SyncTestReports(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Foglio Test Report";

        #endregion Properties

        #region Methods

        protected override void ConfigureWorkbookParameters()
        {
            OriginInfo = new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx");
            BackupInfo = new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ListaReport");
            FileName = "ListaReport.xlsx";
            RowsToSkip = 3;
        }

        protected override TestReportDto GetDtoFromEntity(TestReport entity) => new TestReportDto(entity);

        protected override IQueryable<TestReport> GetExportingRecordsQuery() => base.GetExportingRecordsQuery()
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(odd => odd.Material)
                        .ThenInclude(mat => mat.ColorComponent)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                .ThenInclude(odd => odd.Material)
                    .ThenInclude(mat => mat.MaterialFamily)
                        .ThenInclude(mtf => mtf.L1)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(odd => odd.Material)
                        .ThenInclude(mat => mat.MaterialCustomer)
                            .ThenInclude(mac => mac.Customer)
            .Include(trp => trp.Order)
                .ThenInclude(odd => odd.OrderData)
                .ThenInclude(ord => ord.Material)
                    .ThenInclude(mat => mat.Project)
                        .ThenInclude(prj => prj.WBSRelations)
                            .ThenInclude(wbr => wbr.Up)
                                .ThenInclude(wbu => wbu.WBSRelations)
                                    .ThenInclude(wur => wur.Up)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.InspectionLots)
                    .ThenInclude(inl => inl.InspectionPoints)
                        .ThenInclude(inp => inp.InspectionSpecification)
                            .ThenInclude(ins => ins.InspectionCharacteristic)
            .OrderByDescending(rep => rep.Number);

        protected override IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>()
            {
                new ModifyRangeToken()
                {
                    RangeName = "LastUpdateDateCell",
                    SheetIndex = "Report",
                    Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                }
            };
        }

        protected override IRecordEvaluator<TestReport> GetRecordEvaluator() => new TestReportRecordEvaluator();

        #endregion Methods
    }

    public class TestReportDto : IXmlDto
    {
        public TestReportDto()
        {

        }

        public TestReportDto(TestReport testReport)
        {

            MaterialCode = testReport.Order?.OrderData?.FirstOrDefault().Material?.Code;
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

        #region Properties

        [Column(27), Imported]
        public double? BreakingElongationL { get; set; }

        [Column(28), Imported]
        public double? BreakingElongationT { get; set; }

        [Column(25), Imported]
        public double? BreakingLoadL { get; set; }

        [Column(26), Imported]
        public double? BreakingLoadT { get; set; }

        [Column(19), Imported]
        public string ColorJudgement { get; set; }

        [Column(7)]
        public string ColorName { get; set; }

        [Column(12)]
        public int? ControlPlan { get; set; }

        [Column(13)]
        public string Customer { get; set; }

        [Column(23), Imported]
        public double? DetachForceL { get; set; }

        [Column(24), Imported]
        public double? DetachForceT { get; set; }

        [Column(16)]
        public double? EmbossingavgThickness { get; set; }

        [Column(15)]
        public double? EmbossingAvgWeight { get; set; }

        [Column(22), Imported]
        public string FlammabilityEvaluation { get; set; }

        [Column(20), Imported]
        public double? Gloss { get; set; }

        [Column(21), Imported]
        public double? GlossZ { get; set; }

        [Column(3)]
        public string MaterialCode { get; set; }

        [Column(10), Imported]
        public string Notes { get; set; }

        [Column(14), Imported]
        public string Notes2 { get; set; }

        [Column(1), Imported]
        public int Number { get; set; }

        [Column(4), Imported]
        public string Operator { get; set; }

        [Column(2), Imported]
        public int? OrderNumber { get; set; }

        [Column(5)]
        public string OrderType { get; set; }

        [Column(33), Imported]
        public string OtherTests { get; set; }

        [Column(11)]
        public string PCA { get; set; }

        [Column(6)]
        public double? PlannedOrderQuantity { get; set; }

        [Column(31), Imported]
        public double? SetL { get; set; }

        [Column(32), Imported]
        public double? SetT { get; set; }

        [Column(29), Imported]
        public double? StretchL { get; set; }

        [Column(30), Imported]
        public double? StretchT { get; set; }

        [Column(8)]
        public string Structure { get; set; }

        [Column(18), Imported]
        public double? Thickness { get; set; }

        [Column(9)]
        public string TrialScope { get; set; }

        [Column(17), Imported]
        public double? Weight { get; set; }

        #endregion Properties
    }

    public class TestReportRecordEvaluator : RecordEvaluator<TestReport, int>
    {
        #region Methods

        protected override IRecordValidator<TestReport> GetRecordValidator() => new TestReportRecordValidator();

        protected override int GetIndexKey(TestReport record) => record.Number;

        #endregion Methods
    }

    public class TestReportRecordValidator : IRecordValidator<TestReport>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public TestReport GetInsertableRecord(TestReport record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(ord => ord.Number, ord => ord);
        }

        public bool IsValid(TestReport record) => record.OrderNumber == null || _orderIndex.ContainsKey((int)record.OrderNumber);

        #endregion Methods
    }
}