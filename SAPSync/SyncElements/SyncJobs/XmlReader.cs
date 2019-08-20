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
    public class XmlReader<T, TDto> : XmlInteraction<T, TDto>, IRecordReader<T> where T : class, new() where TDto : class, IXmlDto<T>, new()
    {
        #region Constructors

        public XmlReader(XmlInteractionConfiguration configuration) : base(configuration)
        {
        }
        
        #endregion Constructors

        #region Properties

        public override string Name => "XmlReader";
        
        #endregion Properties

        #region Methods
        
        protected virtual IEnumerable<T> ConvertDtos(IEnumerable<TDto> importedDtos) => importedDtos.Select(dto => GetEntityFromDto(dto));
        
        protected virtual TDto GetDtoFromRow(IEnumerable<ExcelRangeBase> xlRow)
        {
            var output = new TDto();

            foreach (DtoProperty column in GetDtoColumns(new Type[] { typeof(Value), typeof(Imported) }))
            {
                try
                {
                    var cell = xlRow.FirstOrDefault(ce => ce.Start.Column == column.ColumnIndex);
                    Type t = column.Property.PropertyType;

                    if (cell?.Value == null)
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
            foreach (DtoProperty column in GetDtoColumns(new Type[] { typeof(FontColor), typeof(Imported) }))
            {
                try
                {
                    var cell = xlRow.FirstOrDefault(ce => ce.Start.Column == column.ColumnIndex);

                    if (cell == null)
                        column.Property.SetValue(output, null);
                    else 
                        column.Property.SetValue(output, cell.Style.Font.Color);
                }
                catch (Exception e)
                {
                    RaiseSyncError(e: e,
                        errorMessage: e.Message);
                }
            }

            return output;
        }

        protected virtual IEnumerable<TDto> GetDtosFromTable(IEnumerable<ExcelRangeBase> rows)
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


        protected virtual IEnumerable<TDto> ReadFromOrigin()
        {
            IEnumerable<TDto> output;

            using (ExcelPackage xlPackage = new ExcelPackage(Configuration.TargetPath))
            {
                if (!xlPackage.File.Exists)
                    throw new InvalidOperationException("File di origine non trovato: " + xlPackage.File.FullName);

                var t = typeof(TDto);

                IEnumerable<ExcelRangeBase> rows = GetRows(xlPackage.Workbook);

                output = GetDtosFromTable(rows);
            }
            return output;
        }

        public virtual IEnumerable<T> ReadRecords()
        {
            try
            {
                IEnumerable<TDto> importedDtos = ReadFromOrigin();

                return ConvertDtos(importedDtos);
            }
            catch (Exception e)
            {
                RaiseSyncError(e, "Errore di lettura del file", SAPSync.SyncErrorEventArgs.ErrorSeverity.Major);
                return null;
            }
        }

        
        #endregion Methods
    }
}