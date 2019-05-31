using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.Functions
{
    public abstract class ReadTableBase<T>
    {
        #region Fields

        internal string[] _fields;
        internal int _rowCount = 0;
        internal List<string> _selectionOptions = new List<string>();
        internal char[] _separator = new char[] { '|' };
        internal string _tableName;
        private readonly string _functionName = "RFC_READ_TABLE";

        #endregion Fields

        #region Properties

        public ReadTableBatchingOptions BatchingOptions { get; set; }

        #endregion Properties

        #region Methods

        public IList<T> Invoke(RfcDestination rfcDestination)
        {
            ConfigureBatchingOptions();

            if (_fields == null)
                throw new ArgumentNullException("Fields");

            IEnumerable<IRfcFunction> executionStack = GetExecutionStack(rfcDestination);
            List<T> results = new List<T>();

            foreach (IRfcFunction rfcFunction in executionStack)
            {
                rfcFunction.Invoke(rfcDestination);
                IRfcTable output = rfcFunction.GetTable("DATA");
                results.AddRange(ConvertRfcTable(output));
            }

            return results;
        }

        public async Task<IList<T>> InvokeAsync(RfcDestination rfcDestination)
        {
            return await Task.Run(() => Invoke(rfcDestination));
        }

        internal virtual T ConvertDataArray(string[] data) => default(T);

        internal virtual T ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);
            return ConvertDataArray(data);
        }

        internal DateTime DateStringToDate(string date, string time = "")
        {
            DateTime output;

            if (string.IsNullOrEmpty(date) || date.Length != 8 || date == "00000000")
                return new DateTime();

            string yearString = date.Substring(0, 4);
            string monthString = date.Substring(4, 2);
            string dayString = date.Substring(6, 2);

            int year, month, day;
            int hour = 0,
                minute = 0,
                second = 0;

            if (int.TryParse(yearString, out year) &&
                int.TryParse(monthString, out month) &&
                int.TryParse(dayString, out day))
            {
                if (!string.IsNullOrEmpty(time) && time.Length >= 6)
                {
                    string hourString = time.Substring(0, 2);
                    string minuteString = time.Substring(2, 2);
                    string secondString = time.Substring(4, 2);

                    if (hourString == "24")
                        hour = 00;
                    else
                        int.TryParse(hourString, out hour);

                    int.TryParse(minuteString, out minute);
                    int.TryParse(secondString, out second);
                }
            }
            else
                return new DateTime();

            try
            {
                output = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Formato input non valido. /nDate : " + date +
                    "/nTime: " + time,
                    e);
            }

            return output;
        }

        protected virtual void ConfigureBatchingOptions()
        {
        }

        private IList<T> ConvertRfcTable(IRfcTable rfcTable)
        {
            IList<T> results = new List<T>();
            foreach (IRfcStructure row in rfcTable)
            {
                T newEntity = ConvertRow(row);
                if (newEntity != null)
                    results.Add(newEntity);
            }

            return results;
        }

        private IEnumerable<IRfcFunction> GetExecutionStack(RfcDestination rfcDestination)
        {
            List<IRfcFunction> executionStack = new List<IRfcFunction>();

            if (BatchingOptions == null)
                executionStack.Add(GetInitializedFunction(rfcDestination, _selectionOptions));
            else
            {
                long currentValue = BatchingOptions.MinValue;
                long currentMax = (currentValue + BatchingOptions.BatchSize) - 1;

                while (currentValue <= BatchingOptions.MaxValue)
                {
                    if (currentMax > BatchingOptions.MaxValue)
                        currentMax = BatchingOptions.MaxValue;

                    string batchParameters = string.Format("{0} GE '{1}' AND {0} LE '{2}'", new object[] { BatchingOptions.Field, currentValue.ToString("000000000000"), currentMax.ToString("000000000000") });

                    List<string> batchSelection = new List<string>(_selectionOptions);
                    batchSelection.Add(batchParameters);
                    IRfcFunction currentFunction = GetInitializedFunction(rfcDestination, batchSelection);
                    currentValue += BatchingOptions.BatchSize;
                    currentMax += BatchingOptions.BatchSize;
                    executionStack.Add(currentFunction);
                }
            }

            return executionStack;
        }

        private IRfcFunction GetFunction(RfcDestination rfcDestination)
        {
            IRfcFunction rfcFunction;

            RfcRepository rfcRepository = rfcDestination.Repository;
            rfcFunction = rfcRepository.CreateFunction(_functionName);

            return rfcFunction;
        }

        private IRfcFunction GetInitializedFunction(RfcDestination rfcDestination, IEnumerable<string> selectionOptions = null)
        {
            IRfcFunction rfcFunction = GetFunction(rfcDestination);

            rfcFunction.SetValue("query_table", _tableName);
            rfcFunction.SetValue("delimiter", _separator[0]);
            rfcFunction.SetValue("rowcount", _rowCount);

            IRfcTable rfcOptions = rfcFunction.GetTable("Options");

            if (selectionOptions != null)
                foreach (string option in selectionOptions)
                {
                    rfcOptions.Append();
                    rfcOptions.SetValue("TEXT", option);
                }

            IRfcTable rfcFields = rfcFunction.GetTable("fields");

            foreach (string column in _fields)
            {
                rfcFields.Append();
                rfcFields.SetValue("fieldname", column);
            }

            return rfcFunction;
        }

        #endregion Methods
    }

    public class ReadTableBatchingOptions
    {
        #region Properties

        public long BatchSize { get; set; } = 10000;
        public string Field { get; set; } = "";
        public long MaxValue { get; set; } = 999999;
        public long MinValue { get; set; } = 0;

        #endregion Properties
    }
}