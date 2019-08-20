using OfficeOpenXml;
using OfficeOpenXml.Style;
using SAPSync.SyncElements.ExcelWorkbooks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAPSync.SyncElements.SyncJobs
{
    public class XlFormatToken
    {
        public XlFormatToken()
        {

        }

        public ExcelAddress RangeAddress { get; set; } = new ExcelAddress(1, 1, 1, 1);

        public ExcelBorderStyle LeftBorderStyle { get; set; } = ExcelBorderStyle.None;
        public ExcelBorderStyle TopBorderStyle { get; set; } = ExcelBorderStyle.None;
        public ExcelBorderStyle BottomBorderStyle { get; set; } = ExcelBorderStyle.None;
        public ExcelBorderStyle RightBorderStyle { get; set; } = ExcelBorderStyle.None;

    }


    public class XmlInteractionConfiguration
    {
        public XmlInteractionConfiguration(
            FileInfo targetPath,
            string worksheetName,
            int startRow = 1,
            DirectoryInfo backupFolderPath = null
            )
        {
            TargetPath = targetPath;
            BackupFolderPath = backupFolderPath;
            WorksheetName = worksheetName;
            StartRow = startRow;
            
        }

        public Color ImportedColumnFill { get; set; } = Color.Transparent;
        
        public FileInfo TargetPath { get; }
        public DirectoryInfo BackupFolderPath { get; }
        public string WorksheetName { get; }
        public string UpdateTimeRangeName { get; set; } = "LastUpdateRange";
        public int StartRow { get; }
    }

    public class XmlWriter<T, TDto> : XmlInteraction<T, TDto>, IRecordWriter<T> where T : class where TDto : class, IXmlDto<T>, new()
    {
        public XmlWriter(XmlInteractionConfiguration configuration,
            string lastUpdateRangeName = "LastUpdateRange") : base(configuration)
        {
            
        }

        public string LastUpdateRangeName { get; }


        public override string Name => "XmlWriter";

        #region Properties

        public void WriteRecords(IEnumerable<T> records) => Execute(records);

        #endregion Properties

        #region Methods

        protected virtual void ApplyModifyRangeToken(ExcelWorkbook xlWorkbook, ExcelWorkbooks.ModifyRangeToken token)
        {
            if (token.RangeName != null)
                xlWorkbook.Names[token.RangeName].Value = token.Value;
            else
            {
                ExcelWorksheet xlWorkSheet = xlWorkbook.Worksheets[token.SheetIndex];
                ExcelRangeBase xlRange = xlWorkSheet.Cells[token.StartCell.Item1, token.StartCell.Item2, token.EndCell.Item1, token.EndCell.Item2];
                xlRange.Value = token.Value;
            }
        }

        protected virtual void ApplyModifyRangeTokens(ExcelWorkbook xlWorkbook)
        {
            foreach (ModifyRangeToken token in GetRangesToModify())
                try
                {
                    ApplyModifyRangeToken(xlWorkbook, token);
                }
                catch(Exception e)
                {
                    RaiseSyncError(e:e,
                        errorMessage: "Impossibile applicare token modifica range");
                }
        }


        protected virtual void CreateBackup()
        {
            try
            {
                if (Configuration.BackupFolderPath != null)
                    File.Copy(Configuration.TargetPath.FullName, Configuration.BackupFolderPath.FullName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + Configuration.BackupFolderPath.Name, true);
                else
                    File.Copy(Configuration.TargetPath.FullName, Configuration.TargetPath.DirectoryName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + Configuration.TargetPath.Name, true);
            }
            catch (Exception e)
            {
                RaiseSyncError(e,
                    "Impossibile effettuare Backup",
                    SyncErrorEventArgs.ErrorSeverity.Minor);
                throw new InvalidOperationException("Impossibile accedere al file:" + e.Message, e);
            }
        }

        protected virtual void Execute(IEnumerable<T> records)
        {
            CreateBackup();
            IEnumerable<TDto> exportDtos = GetDtosFromEntities(records);
            WriteToOrigin(exportDtos);
        }



        protected virtual TDto GetDtoFromEntity(T entity)
        {
            TDto output = new TDto();
            output.SetValues(entity);
            return output;
        }

        protected virtual IEnumerable<TDto> GetDtosFromEntities(IEnumerable<T> records) => records.Select(rec => GetDtoFromEntity(rec)).ToList();
               
        protected virtual IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>();
        }

        protected virtual void SetFormats(ExcelWorksheet xlWorksheet)
        {
        }

        protected virtual void WriteLastUpdateTime(ExcelWorkbook xlWorkbook)
        {
            if (xlWorkbook.Names.ContainsKey(Configuration.UpdateTimeRangeName))
                xlWorkbook.Names[Configuration.UpdateTimeRangeName].Value = DateTime.Now;
        }

        protected virtual void WriteToOrigin(IEnumerable<TDto> dtos)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(Configuration.TargetPath))
            {
                ExcelWorksheet xlWorksheet = xlPackage.Workbook.Worksheets[Configuration.WorksheetName] ?? throw new ArgumentException("Il Foglio specificato non esiste");
                int currentRow = Configuration.StartRow;
                int maxcol = GetDtoColumns(new Type[] { typeof(Value), typeof(Exported) }).Select(co => co.ColumnIndex).Max();

                XlFormatToken format = new XlFormatToken();

                xlWorksheet.DeleteRow(currentRow, xlWorksheet.Dimension.End.Row - currentRow + 1);

                foreach (TDto dto in dtos)
                {
                    foreach (DtoProperty column in GetDtoColumns(new Type[] { typeof(Value), typeof(Exported) }))
                        xlWorksheet.Cells[currentRow, column.ColumnIndex].Value = column.Property.GetValue(dto);


                    xlWorksheet.Cells[currentRow, 1, currentRow, maxcol].Style.Border.Left.Style = format.LeftBorderStyle;
                    xlWorksheet.Cells[currentRow, 1, currentRow, maxcol].Style.Border.Right.Style = format.RightBorderStyle;
                    xlWorksheet.Cells[currentRow, 1, currentRow, maxcol].Style.Border.Top.Style = format.TopBorderStyle;
                    xlWorksheet.Cells[currentRow, 1, currentRow, maxcol].Style.Border.Bottom.Style = format.BottomBorderStyle;

                    foreach (DtoProperty column in GetDtoColumns(new Type[] { typeof(Imported), typeof(Value) }))
                    {
                        xlWorksheet.Cells[currentRow, column.ColumnIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        xlWorksheet.Cells[currentRow, column.ColumnIndex].Style.Fill.BackgroundColor.SetColor(Configuration.ImportedColumnFill);
                    }

                    foreach (DtoProperty column in GetDtoColumns(new Type[] { typeof(FontColor), typeof(Exported) }))
                        xlWorksheet.Cells[currentRow, column.ColumnIndex].Style.Font.Color.SetColor((Color)column.Property.GetValue(dto));

                    currentRow++;
                }

                WriteLastUpdateTime(xlPackage.Workbook);
                ApplyModifyRangeTokens(xlPackage.Workbook);

                xlPackage.SaveAs(Configuration.TargetPath);
            }
        }

        #endregion Methods
    }
}