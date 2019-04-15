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
        internal char[] _separator = new char[] { '|' };
        internal string _tableName;
        private readonly string _functionName = "RFC_READ_TABLE";
        private IRfcFunction _rfcFunction;

        #endregion Fields

        #region Methods

        public IList<T> Invoke(RfcDestination rfcDestination)
        {
            if (_fields == null)
                throw new ArgumentNullException("Fields");

            InitializeFunction(rfcDestination);
            InitializeParameters();
            _rfcFunction.Invoke(rfcDestination);
            IRfcTable output = _rfcFunction.GetTable("DATA");
            IList<T> results = ConvertRfcTable(output);

            return results;
        }

        public async Task<IList<T>> InvokeAsync(RfcDestination rfcDestination)
        {
            return await Task.Run(() => Invoke(rfcDestination));
        }

        internal abstract T ConvertRow(IRfcStructure row);

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

        private void InitializeFunction(RfcDestination rfcDestination)
        {
            RfcRepository rfcRepository = rfcDestination.Repository;
            _rfcFunction = rfcRepository.CreateFunction(_functionName);
        }

        private void InitializeParameters()
        {
            _rfcFunction.SetValue("query_table", _tableName);
            _rfcFunction.SetValue("delimiter", _separator[0]);
            _rfcFunction.SetValue("rowcount", _rowCount);

            IRfcTable rfcFields = _rfcFunction.GetTable("fields");

            foreach (string column in _fields)
            {
                rfcFields.Append();
                rfcFields.SetValue("fieldname", column);
            }
        }

        #endregion Methods
    }
}