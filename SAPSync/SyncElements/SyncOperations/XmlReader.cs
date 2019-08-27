using OfficeOpenXml;
using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SAPSync.SyncElements.SyncOperations
{
    public class XmlReader<T, TDto> : XmlInteraction<T, TDto>, IRecordReader<T> where T : class, new() where TDto : class, IXmlDto<T>, new()
    {
        #region Constructors

        public XmlReader(XmlInteractionConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Events

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        public event EventHandler ReadCompleted;

        public event EventHandler<RecordPacketCompletedEventArgs<T>> RecordPacketCompleted;

        #endregion Events

        #region Methods

        public void CloseReader()
        {
        }

        public void OpenReader()
        {
        }

        public virtual void ReadRecords()
        {
            try
            {
                IEnumerable<TDto> importedDtos = ReadFromOrigin();

                IEnumerable<T> records = ConvertDtos(importedDtos);
                RaisePacketCompleted(records);
            }
            catch (Exception e)
            {
                RaiseReadError(e, "Errore di lettura del file");
            }
            finally
            {
                RaiseReadCompleted();
            }
        }

        public void StartReadAsync() => StartChildTask(() => ReadRecords());

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
                    RaiseReadError(e: e,
                        message: e.Message);
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
                    RaiseReadError(e, e.Message);
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

        protected virtual void RaisePacketCompleted(IEnumerable<T> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<T>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());

        protected virtual void RaiseReadError(Exception e, string message = null)
        {
            ErrorRaised?.Invoke(this, new SyncErrorEventArgs());
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

        #endregion Methods
    }
}