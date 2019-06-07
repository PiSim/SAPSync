using SSMD;
using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace SAPSync.SyncElements
{
    public class ModifyRangeToken
    {
        #region Properties

        public string RangeIndex { get; set; }
        public string SheetIndex { get; set; }
        public object Value { get; set; }

        #endregion Properties
    }

    public abstract class SyncExcelWorkbook : SyncElement<object>
    {
        #region Constructors

        public SyncExcelWorkbook(SSMDData sSMDData) : base(sSMDData)
        {
        }

        #endregion Constructors

        #region Properties

        protected string BackupFolder { get; set; }

        protected string FileName { get; set; }

        protected string FullOriginPath => OriginFolder + "\\" + FileName;

        protected string OriginFolder { get; set; }

        protected string OutputFolder { get; set; }

        protected string UnprotectPassword { get; set; }

        #endregion Properties

        #region Methods

        protected abstract void ConfigureWorkbookParameters();

        protected virtual void CreateBackup()
        {
            try
            {
                File.Copy(FullOriginPath, BackupFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + FileName, true);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Impossibile accedere al file");
            }
        }

        protected virtual void ExecuteUpdateSequence(Excel.Workbook xlWorkBook)
        {
            UnProtectWorkBook(xlWorkBook);
            xlWorkBook.RefreshAll();
            UpdateRanges(xlWorkBook);
            ProtectWorkBook(xlWorkBook);
        }

        protected virtual IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            ConfigureWorkbookParameters();
        }

        protected virtual void ProtectWorkBook(Excel.Workbook xlWorkBook)
        {
            xlWorkBook.Protect(UnprotectPassword);
            foreach (Excel.Worksheet ws in xlWorkBook.Sheets)
                ws.Protect(UnprotectPassword);
        }

        protected virtual void RunMacro()
        {
            Excel.Application xlApp = new Excel.Application();
            try
            {
                Excel.Workbook xlWorkBook;
                //~~> Start Excel and open the workbook.
                xlWorkBook = xlApp.Workbooks.Open(FullOriginPath);
                ExecuteUpdateSequence(xlWorkBook);
                SaveAndClose(xlWorkBook);
            }
            finally
            {
                //~~> Quit the Excel Application
                xlApp.Quit();
            }
        }

        protected override void RunSyncronizationSequence()
        {
            ChangeStatus("In Aggiornamento");
            CreateBackup();
            RunMacro();
        }

        protected virtual bool SaveAndClose(Excel.Workbook xlWorkBook)
        {
            try
            {
                xlWorkBook.Close(true);
                return true;
            }
            catch (Exception e)
            {
                xlWorkBook.Close(false);
                string errorMessage = "Impossibile salvare il report: " + e.Message;
                RaiseSyncError(errorMessage);
                throw new InvalidOperationException(errorMessage, e);
            }
        }

        protected virtual void UnProtectWorkBook(Excel.Workbook xlWorkBook)
        {
            xlWorkBook.Unprotect(UnprotectPassword);
            foreach (Excel.Worksheet ws in xlWorkBook.Sheets)
                ws.Unprotect(UnprotectPassword);
        }

        protected virtual void UpdateRanges(Excel.Workbook workbook)
        {
            foreach (ModifyRangeToken rangeToken in GetRangesToModify())
            {
                try
                {
                    Excel.Range xlRange = workbook.Names.Item(rangeToken.RangeIndex).RefersToRange;
                    xlRange.Value = rangeToken.Value;
                }
                catch (Exception e)
                {
                    RaiseSyncError(string.Format("Impossibile modificare il range \n WorksheetIndex = {0} \nRangeIndex = {1} \nValue = {2}\n\n", new object[] { rangeToken.SheetIndex, rangeToken.SheetIndex, rangeToken.Value }));
                }
            }
        }

        #endregion Methods
    }
}