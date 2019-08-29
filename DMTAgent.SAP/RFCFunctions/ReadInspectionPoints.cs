using SAP.Middleware.Connector;
using SSMD;
using System;
using System.Globalization;

namespace DMTAgent.SAP
{
    public class ReadInspectionPoints : ReadTableBase<InspectionPoint>
    {
        #region Constructors

        public ReadInspectionPoints() : base()
        {
            _tableName = "QASR";
            _fields = new string[]
            {
                "PRUEFLOS",
                "VORGLFNR",
                "MERKNR",
                "PROBENR",
                "PRUEFDATUB",
                "MAXWERT",
                "MITTELWERT",
                "MINWERT"
            };
        }

        #endregion Constructors

        #region Methods

        internal override InspectionPoint ConvertRow(IRfcStructure row)
        {
            InspectionPoint output = new InspectionPoint();

            string[] data = row.GetString("WA").Split(_separator);

            DateTime inspectionDate = DateStringToDate(data[4]);

            if (!long.TryParse(data[0], out long lotNumber) ||
                !int.TryParse(data[1], out int nodeNumber) ||
                !int.TryParse(data[2], out int charNumber) ||
                !int.TryParse(data[3], out int sampleNumber) ||
                !double.TryParse(data[5], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double maxValue) ||
                !double.TryParse(data[6], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double avgValue) ||
                !double.TryParse(data[7], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double minValue))
                return null;

            output = new InspectionPoint()
            {
                InspectionLotNumber = lotNumber,
                NodeNumber = nodeNumber,
                CharNumber = charNumber,
                SampleNumber = sampleNumber,
                AvgValue = avgValue,
                InspectionDate = inspectionDate,
                MaxValue = maxValue,
                MinValue = minValue,
            };

            return output;
        }

        #endregion Methods
    }
}