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
    public class XmlReader<T, TDto> : SyncElementBase, IRecordReader<T> where T : class, new() where TDto : class, IXmlDto, new()
    {
        #region Constructors

        public XmlReader(FileInfo sourcePath,
            string sourceRangeName = "MainRange")
        {
            SourcePath = sourcePath;
            SourceRangeName = sourceRangeName;
        }

        public FileInfo SourcePath { get; }
        public string SourceRangeName { get; }

        #endregion Constructors

        #region Properties

        public override string Name => "XmlReader";
        
        #endregion Properties

        #region Methods
        
        protected virtual IEnumerable<T> ConvertDtos(IEnumerable<TDto> importedDtos) => importedDtos.Select(dto => GetEntityFromDto(dto));
        
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
        
        protected virtual TDto GetDtoFromRow(IEnumerable<ExcelRangeBase> xlRow)
        {
            var output = new TDto();

            foreach (DtoProperty column in GetImportedDtoColumns())
            {
                try
                {
                    var cell = xlRow.ElementAt(column.ColumnIndex);
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

        protected virtual IEnumerable<TDto> GetDtosFromTable(IEnumerable<IEnumerable<ExcelRangeBase>> rows)
        {
            var output = rows.Select(row => GetDtoFromRow(row))
                .ToList();

            return output;
        }

        protected virtual T GetEntityFromDto(TDto dto)
        {
            T output = new T();
            PropertyInfo[] tProperties = typeof(T).GetProperties();
            PropertyInfo[] dtoProperties = typeof(TDto).GetProperties();

            foreach (PropertyInfo tproperty in tProperties)
                if (dtoProperties.Any(pro => pro.Name == tproperty.Name))
                    tproperty.SetValue(output, dtoProperties.First(p => p.Name == tproperty.Name).GetValue(dto));

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

        protected virtual IEnumerable<TDto> ReadFromOrigin()
        {
            IEnumerable<TDto> output;

            using (ExcelPackage xlPackage = new ExcelPackage(SourcePath))
            {
                if (!xlPackage.File.Exists)
                    throw new InvalidOperationException("File di origine non trovato: " + xlPackage.File.FullName);

                try
                {
                    ExcelRangeBase sourceRange = xlPackage.Workbook.Names[SourceRangeName];
                    IEnumerable<IEnumerable<ExcelRangeBase>> rows = sourceRange.GroupBy(cc => cc.Start.Row).OrderBy(ca => ca.Key).Select(cb => cb.ToList());

                    output = GetDtosFromTable(rows);
                }
                catch ( Exception e)
                {
                    RaiseSyncError(e, "Errore di lettura del file", SyncService.SyncErrorEventArgs.ErrorSeverity.Major);
                    throw new Exception(e.Message, e);
                }
            }
            return output;
        }

        public virtual IEnumerable<T> ReadRecords()
        {
            IEnumerable<TDto> importedDtos = ReadFromOrigin();

            return ConvertDtos(importedDtos);
        }
        
        #endregion Methods
    }
}