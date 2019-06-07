using SSMD;
using System;
using System.Collections.Generic;

namespace SAPSync.SyncElements.ExcelWorkbooks
{
    public class SyncTrialMasterReport : SyncExcelWorkbook
    {
        #region Constructors

        public SyncTrialMasterReport(SSMDData sSMDData) : base(sSMDData)
        {
            Name = "Foglio Prove";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
        }

        protected override void InitializeRecordEvaluator()
        {
        }

        protected override void EnsureInitialized()
        {
        }

        protected override void ConfigureWorkbookParameters()
        {
            OriginFolder = "L:\\LABORATORIO";
            FileName = "StatoProve.xlsx";
            BackupFolder = "L:\\LABORATORIO\\BackupReport\\StatoProve";
            UnprotectPassword = "vulcalab";
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
                },
                new ModifyRangeToken()
                {
                    SheetIndex ="Foglioprova",
                    RangeIndex = "Provaerrore",
                    Value = "Provaerrore"
                }
            };
        }

        protected override IEnumerable<object> ReadRecords()
        {
            return null;
        }

        #endregion Methods
    }
}