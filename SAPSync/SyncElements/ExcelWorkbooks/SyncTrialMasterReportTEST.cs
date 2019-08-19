using Microsoft.EntityFrameworkCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAPSync.SyncElements.ExcelWorkbooks
{
    public class SyncTrialMasterReportTEST : SyncXmlReport<OrderData, TrialMasterDataDto>
    {
        #region Constructors

        public SyncTrialMasterReportTEST(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Stato Prove - TESTXML";

        #endregion Properties

        #region Methods

        protected override void ConfigureWorkbookParameters()
        {
            BackupFolder = "\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\BackupReport\\StatoOdpProva";
            FileName = "StatoOdpProva.xlsx";
            OriginInfo = new System.IO.FileInfo("L:\\LABORATORIO\\StatoOdpProva.xlsx");
            RowsToSkip = 3;
        }

        protected override TrialMasterDataDto GetDtoFromEntity(OrderData entity)
        {
            TrialMasterDataDto output = base.GetDtoFromEntity(entity);

            output.ColorName = entity.Material?.ColorComponent?.Description;
            output.TrialScope = entity.Order.WorkPhaseLabData?.FirstOrDefault()?.TrialScope;
            output.MaterialFamilyL1Code = entity.Material?.MaterialFamily?.L1?.Code;

            var mainConfirmations = entity.Order.OrderConfirmations?
                .Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'C' || orc.WorkCenter.ShortName[0] == 'S'));

            var laqueringConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'P'));
            var embossingConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'G'));
            var rollingConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'R'));

            if (mainConfirmations != null && mainConfirmations.Count() != 0)
                output.MainProcessingDate = mainConfirmations.Max(con => con.EndTime);
            else
                output.MainProcessingDate = null;

            if (laqueringConfirmations != null && laqueringConfirmations.Count() != 0)
                output.LaqueringDate = laqueringConfirmations.Max(con => con.EndTime);
            else
                output.LaqueringDate = null;

            if (embossingConfirmations != null && embossingConfirmations.Count() != 0)
                output.EmbossingDate = embossingConfirmations.Max(con => con.EndTime);
            else
                output.EmbossingDate = null;

            if (rollingConfirmations != null && rollingConfirmations.Count() != 0)
                output.RollingDate = rollingConfirmations.Max(con => con.EndTime);
            else
                output.RollingDate = null;

            output.IsWithdrawal = (output.MainProcessingDate == null) && (output.LaqueringDate != null || output.EmbossingDate != null || output.RollingDate != null);
            output.PCA = entity.Material?.Project?.WBSRelations?.FirstOrDefault()?.Up?.WBSRelations?.FirstOrDefault()?.Up?.Code;
            output.CustomerName = entity.Material?.MaterialCustomer?.FirstOrDefault()?.Customer?.Name;
            output.PartName = entity.Material?.Project?.WBSRelations?.FirstOrDefault()?.Up?.WBSRelations?.FirstOrDefault()?.Up?.Description2;

            output.ControlPlanNumber = entity.Material?.ControlPlan;
            output.MaterialCode = entity.Material?.Code;
            output.TestReportNumber = entity.Order.TestReports?.FirstOrDefault()?.Number;

            var embossingThicknessControlPoints = entity.Order.InspectionLots?.SelectMany(inl => inl.InspectionPoints).Where(inp => Regex.IsMatch(inp.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOSPES"));
            var embossingWeightControlPoints = entity.Order.InspectionLots?.SelectMany(inl => inl.InspectionPoints).Where(inp => Regex.IsMatch(inp.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOPES"));

            if (embossingThicknessControlPoints != null && embossingThicknessControlPoints.Count() != 0)
                output.EmbossingThicknessValue = embossingThicknessControlPoints.Sum(inp => inp.AvgValue);
            else
                output.EmbossingThicknessValue = 0;

            if (embossingWeightControlPoints != null && embossingWeightControlPoints.Count() != 0)
                output.EmbossingWeightValue = embossingWeightControlPoints.Sum(inp => inp.AvgValue);
            else
                output.EmbossingWeightValue = 0;

            output.FabricCode = entity.Order.OrderComponents?.FirstOrDefault(com => Regex.IsMatch(com.Component.Name, "^P01T"))?.Component?.Name;

            return output;
        }

        protected override IQueryable<OrderData> GetExportingRecordsQuery() => base.GetExportingRecordsQuery()
            .Where(ord => ord.Order.OrderType[0] == 'Z' && ord.OrderNumber < 2000000)
            .Include(ord => ord.Order.InspectionLots)
                .ThenInclude(inl => inl.InspectionPoints)
                    .ThenInclude(inp => inp.InspectionSpecification)
                        .ThenInclude(ins => ins.InspectionCharacteristic)
            .Include(ord => ord.Material.ColorComponent)
            .Include(ord => ord.Material)
                .ThenInclude(mat => mat.MaterialCustomer)
                    .ThenInclude(mac => mac.Customer)
            .Include(ord => ord.Order.OrderComponents)
                .ThenInclude(oc => oc.Component)
            .Include(ord => ord.Material.MaterialFamily.L1)
            .Include(ord => ord.Material.Project.WBSRelations)
                .ThenInclude(wbs => wbs.Up.WBSRelations)
                    .ThenInclude(wbu => wbu.Up.WBSRelations)
            .Include(ord => ord.Material.Project)
                .ThenInclude(prj => prj.WBSDownRelations)
                    .ThenInclude(prj => prj.Down)
            .Include(prj => prj.Order.TestReports)
            .Include(ord => ord.Order.OrderConfirmations)
                .ThenInclude(orc => orc.WorkCenter)
            .Include(ord => ord.Order.WorkPhaseLabData)
            .OrderByDescending(ord => ord.OrderNumber);

        protected override IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>()
            {
                new ModifyRangeToken()
                {
                    RangeName = "LastUpdateCell",
                    SheetIndex = "Master Prove",
                    Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                }
            };
        }

        protected override IRecordEvaluator<OrderData> GetRecordEvaluator() => new TrialMasterEvaluator();

        #endregion Methods
    }

    public class TrialMasterDataDto : IXmlDto
    {
        #region Properties

        [Column(7)]
        public string ColorName { get; set; }

        [Column(20)]
        public int? ControlPlanNumber { get; set; }

        [Column(15)]
        public string CustomerName { get; set; }

        [Column(11)]
        public DateTime? EmbossingDate { get; set; }

        [Column(23)]
        public double EmbossingThicknessValue { get; set; }

        [Column(24)]
        public double EmbossingWeightValue { get; set; }

        [Column(22)]
        public string FabricCode { get; set; }

        [Column(3), Imported]
        public string HasArrivedString
        {
            get => (HasSampleArrived) ? "SI" : "NO";
            set
            {
                HasSampleArrived = (value?.Trim() == "SI");
            }
        }

        public bool HasSampleArrived { get; set; }

        [Column(13)]
        public bool IsWithdrawal { get; set; }

        [Column(10)]
        public DateTime? LaqueringDate { get; set; }

        [Column(9)]
        public DateTime? MainProcessingDate { get; set; }

        [Column(6)]
        public string MaterialCode { get; set; }

        [Column(8)]
        public string MaterialFamilyL1Code { get; set; }

        [Column(1), Imported]
        public int OrderNumber { get; set; }

        [Column(18)]
        public string OrderType { get; set; }

        [Column(16)]
        public string PartName { get; set; }

        [Column(14)]
        public string PCA { get; set; }

        [Column(19)]
        public double PlannedQuantity { get; set; }

        [Column(17)]
        public string ProjectDescription { get; set; }

        [Column(12)]
        public DateTime? RollingDate { get; set; }

        [Column(4), Imported]
        public DateTime? SampleArrivalDate { get; set; }

        [Column(5), Imported]
        public string SampleRollStatus { get; set; }

        [Column(2)]
        public int? TestReportNumber { get; set; }

        [Column(21)]
        public string TrialScope { get; set; }

        #endregion Properties
    }
}