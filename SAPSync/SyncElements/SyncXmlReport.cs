using DataAccessCore;
using OfficeOpenXml;
using SSMD;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAPSync.SyncElements
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

    public abstract class SyncXmlReport<T, TDto> : SyncElement<T> where T : class, new() where TDto : class, IXmlDto, new()
    {
        #region Constructors

        public SyncXmlReport(SSMDData sSMDData) : base(sSMDData)
        {
        }

        #endregion Constructors

        #region Properties

        public int RowsToSkip { get; protected set; }

        protected string BackupFolder { get; set; }

        protected string FileName { get; set; }

        protected string FullOriginPath => OriginFolder + "\\" + FileName;

        protected string OriginFolder { get; set; }

        protected FileInfo OriginInfo { get; set; }

        protected string OutputFolder { get; set; }

        protected string UnprotectPassword { get; set; }

        #endregion Properties

        #region Methods

        protected void ClearRange(ref ExcelWorksheet xlWorkSheet)
        {
            foreach (ExcelRangeBase xlRange in xlWorkSheet.Cells.Where(cell => cell.Start.Row > RowsToSkip))
                xlRange.Value = null;
        }

        protected abstract void ConfigureWorkbookParameters();

        protected virtual IEnumerable<T> ConvertDtos(IEnumerable<TDto> importedDtos) => importedDtos.Select(dto => GetEntityFromDto(dto));

        protected virtual void CreateBackup()
        {
            try
            {
                File.Copy(OriginInfo.FullName, BackupFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_" + FileName, true);
            }
            catch (Exception e)
            {
                SyncFailure();
                throw new InvalidOperationException("Impossibile accedere al file");
            }
        }

        protected override void ExecuteExport(IEnumerable<T> records)
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
                var cell = xlWorksheet.Cells[row, column.ColumnIndex];
                Type t = column.Property.PropertyType;

                if (cell.Value == null)
                    column.Property.SetValue(output, null);
                else if (column.Property.PropertyType == typeof(int))
                    column.Property.SetValue(output, cell.GetValue<int>());
                else if (column.Property.PropertyType == typeof(double))
                    column.Property.SetValue(output, cell.GetValue<double>());
                else if (column.Property.PropertyType == typeof(DateTime))
                    column.Property.SetValue(output, cell.GetValue<DateTime>());
                else if (column.Property.PropertyType == typeof(string))
                    column.Property.SetValue(output, cell.GetValue<string>());
                else
                    throw new InvalidOperationException("Impossibile convertire la riga, tipo non supportato : " + column.Property.PropertyType.ToString());
            }

            return output;
        }

        protected virtual IEnumerable<TDto> GetDtosFromTable(ExcelWorksheet xlWorksheet, IEnumerable<int> rows)
        {
            var output = rows.Skip(RowsToSkip)
                .Select(
                row => GetDtoFromRow(xlWorksheet, row))
                .ToList();

            return output;
        }

        protected virtual T GetEntityFromDto(TDto dto)
        {
            T output = new T();
            PropertyInfo[] tProperties = typeof(T).GetProperties();
            PropertyInfo[] dtoProperties = typeof(TDto).GetProperties();

            foreach (PropertyInfo tproperty in tProperties)
                if (dtoProperties.Any(pro => pro.Name == tproperty.Name && pro.PropertyType == tproperty.PropertyType))
                    tproperty.SetValue(output, dtoProperties.First(p => p.Name == tproperty.Name).GetValue(dto));

            return output;
        }

        protected override void ExportData()
        {
            CreateBackup();
            base.ExportData();
        }

        protected virtual IEnumerable<TDto> GetDtosFromEntities(IEnumerable<T> records) => records.Select(rec => GetDtoFromEntity(rec)).ToList();

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

        protected override void Initialize()
        {
            base.Initialize();
            ConfigureWorkbookParameters();
        }

        protected virtual IEnumerable<TDto> ReadFromOrigin()
        {
            IEnumerable<TDto> output;

            using (ExcelPackage xlPackage = new ExcelPackage(OriginInfo))
            {
                if (!xlPackage.File.Exists)
                    throw new InvalidOperationException("File di origine non trovato: " + xlPackage.File.FullName);

                ExcelWorksheet xlWorkSheet = xlPackage.Workbook.Worksheets[1];
                
                var rows = xlWorkSheet.Cells
                   .Select(cell => cell.Start.Row)
                   .Distinct()
                   .OrderBy(x => x)
                    .Where(x => x > RowsToSkip);

                output = GetDtosFromTable(xlWorkSheet, rows);
            }

            return output;
        }

        protected override IEnumerable<T> ReadRecords()
        {
            IEnumerable<TDto> importedDtos = ReadFromOrigin();

            return ConvertDtos(importedDtos);
        }

        protected virtual void SetRowValuesFromDto(ExcelWorksheet xlWorksheet, int row, TDto dto)
        {
            foreach (DtoProperty column in GetDtoColumns())
            {
                var cell = xlWorksheet.Cells[row, column.ColumnIndex];
                cell.Value = column.Property.GetValue(dto);
            }
        }

        protected virtual void WriteToOrigin(IEnumerable<TDto> dtos)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(OriginInfo))
            {
                ExcelWorksheet xlWorkSheet = xlPackage.Workbook.Worksheets[1];

                ClearRange(ref xlWorkSheet);

                int startRow = RowsToSkip + 1;

                foreach (TDto dto in dtos)
                    SetRowValuesFromDto(xlWorkSheet, startRow++, dto);

                xlPackage.SaveAs(OriginInfo);
            }
        }

        #endregion Methods
    }
}