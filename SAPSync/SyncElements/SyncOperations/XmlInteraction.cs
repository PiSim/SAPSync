using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.SyncOperations
{


    public abstract class XmlInteraction<T, TDto> : SyncElementBase where T : class where TDto : class, IXmlDto<T>, new()
    {
        public XmlInteraction(XmlInteractionConfiguration configuration)
        {
            Configuration = configuration;
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
        protected virtual IEnumerable<DtoProperty> GetDtoColumns(Type[] types)
        {
            return typeof(TDto)
                .GetProperties()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(Column))
                    && types.All(typ => x.CustomAttributes.Any(y => y.AttributeType == typ)))
                .Select(p => new DtoProperty()
                {
                    Property = p,
                    ColumnIndex = p.GetCustomAttributes<Column>().First().ColumnIndex
                }).ToList();
        }

        protected virtual ExcelRangeBase GetMainRange(ExcelWorkbook xlWorkbook)
        {
            ExcelWorksheet xlWorksheet = xlWorkbook.Worksheets[Configuration.WorksheetName] ?? throw new ArgumentException("Il Foglio specificato non esiste");
            int maxcol = GetDtoColumns(new Type[] { typeof(Value), typeof(Exported) }).Select(co=> co.ColumnIndex).Max();
            return xlWorksheet.Cells[Configuration.StartRow, 1, xlWorksheet.Cells.Rows, maxcol];
        }

        protected virtual IEnumerable<ExcelRangeBase> GetRows(ExcelWorkbook xlWorkbook)
        {
            ExcelWorksheet xlWorksheet = xlWorkbook.Worksheets[Configuration.WorksheetName] ?? throw new ArgumentException("Il Foglio specificato non esiste");
            int maxcol = GetDtoColumns(new Type[] { typeof(Value), typeof(Exported) }).Select(co => co.ColumnIndex).Max();
            int lastRow = xlWorksheet.Dimension.Rows;
            IList<ExcelRangeBase> output = new List<ExcelRangeBase>(lastRow);

            for (int ii = Configuration.StartRow; ii <= lastRow; ii++)
                output.Add(xlWorksheet.Cells[ii, 1, ii, maxcol]);

            return output;
        }

        public XmlInteractionConfiguration Configuration { get; }
    }
}
