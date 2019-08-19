using Microsoft.EntityFrameworkCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAPSync.SyncElements.SyncJobs.Dto
{
    public class TrialMasterDataDto : DtoBase<OrderData>
    {
        public override void SetValues(OrderData entity)
        {
            base.SetValues(entity);

            ColorName = entity.Material?.ColorComponent?.Description;
            TrialScope = entity.Order.WorkPhaseLabData?.FirstOrDefault()?.TrialScope;
            MaterialFamilyL1Code = entity.Material?.MaterialFamily?.L1?.Code;

            var mainConfirmations = entity.Order.OrderConfirmations?
                .Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'C' || orc.WorkCenter.ShortName[0] == 'S'));

            var laqueringConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'P'));
            var embossingConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'G'));
            var rollingConfirmations = entity.Order.OrderConfirmations?.Where(orc => orc.DeletionFlag == false && (orc.WorkCenter.ShortName[0] == 'R'));

            if (mainConfirmations != null && mainConfirmations.Count() != 0)
                MainProcessingDate = mainConfirmations.Max(con => con.EndTime);
            else
                MainProcessingDate = null;

            if (laqueringConfirmations != null && laqueringConfirmations.Count() != 0)
                LaqueringDate = laqueringConfirmations.Max(con => con.EndTime);
            else
                LaqueringDate = null;

            if (embossingConfirmations != null && embossingConfirmations.Count() != 0)
                EmbossingDate = embossingConfirmations.Max(con => con.EndTime);
            else
                EmbossingDate = null;

            if (rollingConfirmations != null && rollingConfirmations.Count() != 0)
                RollingDate = rollingConfirmations.Max(con => con.EndTime);
            else
                RollingDate = null;

            IsWithdrawal = (MainProcessingDate == null) && (LaqueringDate != null || EmbossingDate != null || RollingDate != null);
            PCA = entity.Material?.Project?.WBSRelations?.FirstOrDefault()?.Up?.WBSRelations?.FirstOrDefault()?.Up?.Code;
            CustomerName = entity.Material?.MaterialCustomer?.FirstOrDefault()?.Customer?.Name;
            PartName = entity.Material?.Project?.WBSRelations?.FirstOrDefault()?.Up?.WBSRelations?.FirstOrDefault()?.Up?.Description2;

            ControlPlanNumber = entity.Material?.ControlPlan;
            MaterialCode = entity.Material?.Code;
            TestReportNumber = entity.Order.TestReports?.FirstOrDefault()?.Number;

            var embossingThicknessControlPoints = entity.Order.InspectionLots?.SelectMany(inl => inl.InspectionPoints).Where(inp => Regex.IsMatch(inp.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOSPES"));
            var embossingWeightControlPoints = entity.Order.InspectionLots?.SelectMany(inl => inl.InspectionPoints).Where(inp => Regex.IsMatch(inp.InspectionSpecification?.InspectionCharacteristic?.Name, "^GOPES"));

            if (embossingThicknessControlPoints != null && embossingThicknessControlPoints.Count() != 0)
                EmbossingThicknessValue = embossingThicknessControlPoints.Sum(inp => inp.AvgValue);
            else
                EmbossingThicknessValue = 0;

            if (embossingWeightControlPoints != null && embossingWeightControlPoints.Count() != 0)
                EmbossingWeightValue = embossingWeightControlPoints.Sum(inp => inp.AvgValue);
            else
                EmbossingWeightValue = 0;

            FabricCode = entity.Order.OrderComponents?.FirstOrDefault(com => Regex.IsMatch(com.Component.Name, "^P01T"))?.Component?.Name;

        }

        #region Properties

        [Column(7), Value, Exported]
        public string ColorName { get; set; }

        [Column(20), Value, Exported]
        public int? ControlPlanNumber { get; set; }

        [Column(15), Value, Exported]
        public string CustomerName { get; set; }

        [Column(11), Value, Exported]
        public DateTime? EmbossingDate { get; set; }

        [Column(23), Value, Exported]
        public double EmbossingThicknessValue { get; set; }

        [Column(24), Value, Exported]
        public double EmbossingWeightValue { get; set; }

        [Column(22), Value, Exported]
        public string FabricCode { get; set; }

        [Column(3), Imported, Value, Exported]
        public string HasArrivedString
        {
            get => (HasSampleArrived) ? "SI" : "NO";
            set
            {
                HasSampleArrived = (value?.Trim() == "SI");
            }
        }

        public bool HasSampleArrived { get; set; }

        [Column(13), Value, Exported]
        public bool IsWithdrawal { get; set; }

        [Column(10), Value, Exported]
        public DateTime? LaqueringDate { get; set; }

        [Column(9), Value, Exported]
        public DateTime? MainProcessingDate { get; set; }

        [Column(6), Value, Exported]
        public string MaterialCode { get; set; }

        [Column(8), Value, Exported]
        public string MaterialFamilyL1Code { get; set; }

        [Column(1), Imported, Value, Exported]
        public int OrderNumber { get; set; }

        [Column(18), Value, Exported]
        public string OrderType { get; set; }

        [Column(16), Value, Exported]
        public string PartName { get; set; }

        [Column(14), Value, Exported]
        public string PCA { get; set; }

        [Column(19), Value, Exported]
        public double PlannedQuantity { get; set; }

        [Column(17), Value, Exported]
        public string ProjectDescription { get; set; }

        [Column(12), Value, Exported]
        public DateTime? RollingDate { get; set; }

        [Column(4), Imported, Value, Exported]
        public DateTime? SampleArrivalDate { get; set; }

        [Column(5), Imported, Value, Exported]
        public string SampleRollStatus { get; set; }

        [Column(2), Value, Exported]
        public int? TestReportNumber { get; set; }

        [Column(21), Value, Exported]
        public string TrialScope { get; set; }

        #endregion Properties
    }
}