using SAP.Middleware.Connector;
using SAPSync.Infrastructure;
using SAPSync.SyncElements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.RFCFunctions
{
    public abstract class ReadTableBase<T> :  IRecordReader<T>
    {
        #region Fields

        internal string[] _fields;
        internal int _rowCount = 0;
        internal List<string> _selectionOptions = new List<string>();
        internal char[] _separator = new char[] { '|' };
        internal string _tableName;
        private readonly string _functionName = "RFC_READ_TABLE";

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;
        public event EventHandler<RecordPacketCompletedEventArgs<T>> RecordPacketCompleted;
        public event EventHandler ReadCompleted;

        #endregion Fields

        #region Properties

        public ReadTableBatchingOptions BatchingOptions { get; set; }

        #endregion Properties

        #region Methods

        protected virtual void RaisePacketCompleted(IEnumerable<T> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<T>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());
        

        public void Invoke(RfcDestination rfcDestination)
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
                RaisePacketCompleted(ConvertRfcTable(output));
            }

            RaiseReadCompleted();       
        }

        public async Task InvokeAsync(RfcDestination rfcDestination) => await Task.Run(() => Invoke(rfcDestination));

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

            int hour = 0,
                minute = 0,
                second = 0;

            if (int.TryParse(yearString, out int year) &&
                int.TryParse(monthString, out int month) &&
                int.TryParse(dayString, out int day))
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
                foreach (string batch in BatchingOptions.GetSelectionStrings())
                {
                    List<string> batchSelection = new List<string>(_selectionOptions)
                    {
                        batch
                    };
                    IRfcFunction currentFunction = GetInitializedFunction(rfcDestination, batchSelection);
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

        public async void StartReadAsync() => await Task.Run(() => Invoke((new SAPReader()).GetRfcDestination()));


        #endregion Methods
    }

    public class ReadTableBatchingOptions
    {
        #region Properties

        public long BatchSize { get; set; } = 10000;
        public string Field { get; set; } = "";
        public long MaxValue { get; set; } = 999999;
        public long MinValue { get; set; } = 0;
        public string StringFormat { get; set; } = "";

        #endregion Properties

        public IEnumerable<string> GetSelectionStrings()
        {
            List<string> output = new List<string>();
            long currentValue = MinValue;
            long currentMax = (currentValue + BatchSize) - 1;

            while (currentValue <= MaxValue)
            {
                if (currentMax > MaxValue)
                    currentMax = MaxValue;

                string batchParameters = string.Format("{0} GE '{1}' AND {0} LE '{2}'", new object[] { Field, currentValue.ToString(StringFormat), currentMax.ToString(StringFormat) });
                output.Add(batchParameters);
                currentValue += BatchSize;
                currentMax += BatchSize;
            }

            return output;
        }
    }
}