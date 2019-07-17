using OfficeOpenXml;
using SAPSync.SyncElements.ExcelWorkbooks;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAPSync.SyncElements.SyncJobs
{
    public class Column : System.Attribute
    {
        #region Constructors

        public Column(int column)
        {
            ColumnIndex = column;
        }

        #endregion Constructors

        #region Properties

        public int ColumnIndex { get; set; }

        #endregion Properties
    }

    public class DtoProperty
    {
        #region Properties

        public int ColumnIndex { get; set; }
        public PropertyInfo Property { get; set; }

        #endregion Properties
    }

    public class Imported : Attribute
    {
    }
    
    public class XmlWriterConfiguration
    {
        public XmlWriterConfiguration(
            FileInfo targetPath,
            DirectoryInfo backupFolderPath = null
            )
        {
            TargetPath = targetPath;
            BackupFolderPath = backupFolderPath;
        }

        public FileInfo TargetPath { get; }
        public DirectoryInfo BackupFolderPath { get; }
    }

    public class XmlWriter<T, TDto> : SyncElementBase, IRecordWriter<T> where T : class where TDto : class, IXmlDto, new()
    {

        public XmlWriter(XmlWriterConfiguration configuration,
            string mainRangeName = "MainRange",
            string lastUpdateRangeName = "LastUpdateRange")
        {
            
            Configuration = configuration;
            MainRangeName = mainRangeName;
            LastUpdateRangeName = lastUpdateRangeName;
        }

        public string MainRangeName { get; }
        public string LastUpdateRangeName { get; }

        public XmlWriterConfiguration Configuration {get;}

        public override string Name => "XmlWriter";

        #region Properties

        public void WriteRecords(IEnumerable<T> records)
        {
            throw new NotImplementedException();
        }

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

        protected void ClearRange(ref ExcelRangeBase xlRange) => xlRange.Value = null;

        protected virtual void CreateBackup()
        {
            try
            {
                if (Configuration.BackupFolderPath != null)
                    File.Copy(Configuration.TargetPath.FullName, Configuration.BackupFolderPath.FullName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + Configuration.BackupFolderPath.Name, true);
                else
                    File.Copy(Configuration.TargetPath.FullName, Configuration.BackupFolderPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + Configuration.TargetPath.Name, true);
            }
            catch (Exception e)
            {
                RaiseSyncError(e,
                    "Impossibile effettuare Backup",
                    SyncService.SyncErrorEventArgs.ErrorSeverity.Minor);
                throw new InvalidOperationException("Impossibile accedere al file:" + e.Message, e);
            }
        }

        protected virtual void Execute(IEnumerable<T> records)
        {
            IEnumerable<TDto> exportDtos = GetDtosFromEntities(records);
            WriteToOrigin(exportDtos);
        }

        protected virtual IEnumerable<DtoProperty> GetDtoColumns()
        {
            return typeof(TDto)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(Column)))
                .Select(p => new DtoProperty()
                {
                    Property = p,
                    ColumnIndex = p.GetCustomAttributes<Column>().First().ColumnIndex
                }).ToList();
        }

        protected virtual TDto GetDtoFromEntity(T entity)
        {
            TDto output = new TDto();
            PropertyInfo[] tProperties = typeof(T).GetProperties();
            PropertyInfo[] dtoProperties = typeof(TDto).GetProperties();

            foreach (PropertyInfo dtoproperty in dtoProperties)
                if (tProperties.Any(pro => pro.Name == dtoproperty.Name && pro.PropertyType == dtoproperty.PropertyType))
                    dtoproperty.SetValue(output, tProperties.First(p => p.Name == dtoproperty.Name).GetValue(entity));

            return output;
        }

        protected virtual TDto GetDtoFromRow(ExcelWorksheet xlWorksheet, int row)
        {
            var output = new TDto();

            foreach (DtoProperty column in GetImportedDtoColumns())
            {
                try
                {
                    var cell = xlWorksheet.Cells[row, column.ColumnIndex];
                    Type t = column.Property.PropertyType;

                    if (cell.Value == null)
                        column.Property.SetValue(output, null);
                    else if (column.Property.PropertyType == typeof(int))
                        column.Property.SetValue(output, cell.GetValue<int>());
                    else if (column.Property.PropertyType == typeof(int?))
                        column.Property.SetValue(output, cell.GetValue<int?>());
                    else if (column.Property.PropertyType == typeof(double))
                        column.Property.SetValue(output, cell.GetValue<double>());
                    else if (column.Property.PropertyType == typeof(double?))
                        column.Property.SetValue(output, cell.GetValue<double?>());
                    else if (column.Property.PropertyType == typeof(DateTime))
                        column.Property.SetValue(output, cell.GetValue<DateTime>());
                    else if (column.Property.PropertyType == typeof(DateTime?))
                        column.Property.SetValue(output, cell.GetValue<DateTime?>());
                    else if (column.Property.PropertyType == typeof(string))
                        column.Property.SetValue(output, cell.GetValue<string>());
                    else
                        throw new InvalidOperationException("Impossibile convertire la riga, tipo non supportato : " + column.Property.PropertyType.ToString());
                }
                catch (Exception e)
                {
                    RaiseSyncError(e:e,
                        errorMessage: e.Message);
                }
            }

            return output;
        }

        protected virtual IEnumerable<TDto> GetDtosFromEntities(IEnumerable<T> records) => records.Select(rec => GetDtoFromEntity(rec)).ToList();

        protected virtual IEnumerable<TDto> GetDtosFromTable(ExcelWorksheet xlWorksheet, IEnumerable<int> rows)
        {
            var output = rows.Select(
                row => GetDtoFromRow(xlWorksheet, row))
                .ToList();

            return output;
        }

        protected virtual IEnumerable<DtoProperty> GetImportedDtoColumns()
        {
            return typeof(TDto)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(Column))
                    && x.CustomAttributes.Any(y => y.AttributeType == typeof(Imported)))
                .Select(p => new DtoProperty()
                {
                    Property = p,
                    ColumnIndex = p.GetCustomAttributes<Column>().First().ColumnIndex
                }).ToList();
        }

        protected virtual IEnumerable<ModifyRangeToken> GetRangesToModify()
        {
            return new List<ModifyRangeToken>();
        }

        
        protected virtual void SetRowValuesFromDto(ExcelWorksheet xlWorksheet, int row, TDto dto)
        {
            foreach (DtoProperty column in GetDtoColumns())
            {
                var cell = xlWorksheet.Cells[row, column.ColumnIndex];
                cell.Value = column.Property.GetValue(dto);
            }
        }

        protected virtual ExcelRangeBase GetMainRange(ExcelWorksheet xlWorkSheet) => xlWorkSheet.Cells[MainRangeName];
        protected virtual ExcelRangeBase GetLastUpdateRange(ExcelWorksheet xlWorkSheet) => xlWorkSheet.Cells[LastUpdateRangeName];
               
        protected virtual void WriteToOrigin(IEnumerable<TDto> dtos)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(Configuration.TargetPath))
            {
                ExcelWorksheet xlWorkSheet = xlPackage.Workbook.Worksheets[1];
                ExcelRangeBase mainRange = GetMainRange(xlWorkSheet);
                ClearRange(ref mainRange);

                int startRow = 0;

                foreach (TDto dto in dtos)
                    SetRowValuesFromDto(xlWorkSheet, startRow++, dto);

                ApplyModifyRangeTokens(xlPackage.Workbook);

                xlPackage.SaveAs(Configuration.TargetPath);
            }
        }

        #endregion Methods
    }
}