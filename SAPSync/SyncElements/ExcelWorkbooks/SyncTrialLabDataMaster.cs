using DataAccessCore;
using DataAccessCore.Commands;
using Microsoft.EntityFrameworkCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SAPSync.SyncElements.ExcelWorkbooks
{
    public class SyncTESTODPPROVA : SyncXmlReport<WorkPhaseLabData, WorkPhaseLabDataDto>
    {
        #region Constructors

        public SyncTESTODPPROVA(SSMDData sSMDData) : base(sSMDData)
        {
            Name = "Master Odp di Prova";
            PerformExport = true;
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new WorkPhaseLabDataEvaluator();
        }

        protected override void ConfigureWorkbookParameters()
        {
            OriginFolder = "\\\\vulcaflex.locale\\datid\\Laboratorio\\temp\\Pietro";
            FileName = "ODPProva.xlsx";
            BackupFolder = "L:\\LABORATORIO\\BackupReport\\ODPProva";
            UnprotectPassword = "vulcalab";
            OriginInfo = new System.IO.FileInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\ODPProva.xlsx");
            RowsToSkip = 3;
        }

        protected override IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>()
            {
                new ModifyRangeToken()
                {
                    SheetIndex ="Report",
                    RangeIndex = "LastUpdateDateCell",
                    Value = DateTime.Now.ToString("dd/MM/yyy hh:mm:ss")
                }
            };
        }

        protected override IQueryable<WorkPhaseLabData> GetExportingRecordsQuery() => base.GetExportingRecordsQuery()
            .Include(wpld => wpld.Order.Material.MaterialFamily.L1)
            .Include(wpld => wpld.Order.Material.ColorComponent)
            .Include(wpld => wpld.Order.OrderData)
            .Include(wpld => wpld.Order)
                .ThenInclude(ord => ord.Material)
                    .ThenInclude(mat => mat.Project)
                        .ThenInclude(prj => prj.WBSUpRelations)
                            .ThenInclude(prj => prj.Up)
            .Where(wpld => wpld.OrderNumber < 2000000 && wpld.Order.OrderType[0] == 'Z')
            .OrderByDescending(wpld => wpld.OrderNumber);

        protected override void ExecutePostImportActions()
        {
            base.ExecutePostImportActions();

            List<Order> trialRecords = _sSMDData.RunQuery(new Query<Order, SSMDContext>()).Where(ord => ord.OrderType[0] == 'Z').ToList();
            IDictionary<int,WorkPhaseLabData> workPhaseDataIndex = _sSMDData.RunQuery(new Query<WorkPhaseLabData, SSMDContext>()).ToDictionary(wpld => wpld.OrderNumber, wpld => wpld) ;

            List<WorkPhaseLabData> newTrials = new List<WorkPhaseLabData>();

            foreach (Order trial in trialRecords)
                if (!workPhaseDataIndex.ContainsKey(trial.Number))
                    newTrials.Add(new WorkPhaseLabData()
                    {
                        OrderNumber = trial.Number
                    });

            _sSMDData.Execute(new InsertEntitiesCommand<SSMDContext>(newTrials));
        }

        protected override WorkPhaseLabDataDto GetDtoFromEntity(WorkPhaseLabData entity)
        {

            WorkPhaseLabDataDto dto = base.GetDtoFromEntity(entity);
            dto.MaterialCode = entity.Order?.Material?.Code;
            dto.OrderType = entity.Order?.OrderType;
            dto.Structure = entity.Order?.Material?.MaterialFamily?.L1?.Code;
            dto.Aspect = entity.Order?.Material?.ColorComponent?.Description.Replace("SKIN","");
            dto.PCA = entity.Order?.Material?.Project?.WBSUpRelations?.FirstOrDefault()?.Up?.Code;
            dto.ProjectDescription = entity.Order?.Material?.Project?.WBSUpRelations?.FirstOrDefault()?.Up?.Description2;
            dto.OrderAmount = entity.Order?.OrderData?.FirstOrDefault()?.PlannedQuantity;
            return dto;
        }

        #endregion Methods
    }

    public class WorkPhaseLabDataDto : IXmlDto
    {
        #region Properties

        [Column(16), Imported]
        public string Actions { get; set; }

        [Column(15), Imported]
        public string Analysis { get; set; }

        [Column(5)]
        public string Aspect { get; set; }

        [Column(2)]
        public string MaterialCode { get; set; }

        [Column(12), Imported]
        public string NotesC { get; set; }

        [Column(13), Imported]
        public string NotesG { get; set; }

        [Column(14), Imported]
        public string NotesP { get; set; }

        [Column(11), Imported]
        public string NotesS { get; set; }

        [Column(9)]
        public string PCA { get; set; }

        [Column(10)]
        public string ProjectDescription { get; set; }

        [Column(4)]
        public double? OrderAmount { get; set; }

        [Column(1), Imported]
        public int OrderNumber { get; set; }

        [Column(3)]
        public string OrderType { get; set; }

        [Column(7)]
        public string Structure { get; set; }

        [Column(8), Imported]
        public string TrialScope { get; set; }

        #endregion Properties
    }

    public class WorkPhaseLabDataEvaluator : RecordEvaluator<WorkPhaseLabData, int>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new WorkPhaseLabDataValidator();
        }

        protected override int GetIndexKey(WorkPhaseLabData record) => record.OrderNumber;

        protected override WorkPhaseLabData SetPrimaryKeyForExistingRecord(WorkPhaseLabData record)
        {
            if (_recordIndex.ContainsKey(GetIndexKey(record)))
                record.ID = _recordIndex[GetIndexKey(record)].ID;

            return record;
        }


        #endregion Methods
    }

    public class WorkPhaseLabDataValidator : IRecordValidator<WorkPhaseLabData>
    {
        #region Fields

        private IDictionary<int, Order> _orderIndex;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderIndex != null;

        public WorkPhaseLabData GetInsertableRecord(WorkPhaseLabData record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderIndex = sSMDData.RunQuery(new Query<Order, SSMDContext>()).ToDictionary(rec => rec.Number, rec => rec);
        }

        

        public bool IsValid(WorkPhaseLabData record) => _orderIndex.ContainsKey(record.OrderNumber);

        #endregion Methods
    }
}