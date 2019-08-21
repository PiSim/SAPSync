using SAP.Middleware.Connector;
using SSMD;
using System.Globalization;

namespace SAPSync.RFCFunctions
{
    public class ReadInspectionSpecifications : ReadTableBase<InspectionSpecification>
    {
        public override string Name => "ReadInspectionSpecifications";
        #region Constructors

        public ReadInspectionSpecifications() : base()
        {
            _tableName = "QAMV";

            _fields = new string[]
                {
                    "PRUEFLOS",
                    "VORGLFNR",
                    "MERKNR",
                    "SOLLWERT",
                    "TOLERANZOB",
                    "TOLERANZUN",
                    "MASSEINHSW",
                    "VERWMERKM"
                };
        }

        #endregion Constructors

        #region Methods

        internal override InspectionSpecification ConvertRow(IRfcStructure row)
        {
            InspectionSpecification output;

            string[] data = row.GetString("WA").Split(_separator);

            if (!long.TryParse(data[0], out long lotNumber) ||
                !int.TryParse(data[1], out int nodeNumber) ||
                !int.TryParse(data[2], out int charNumber) ||
                !double.TryParse(data[3], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double target) ||
                !double.TryParse(data[4], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double upperLimit) ||
                !double.TryParse(data[5], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double lowerLimit))
                return null;

            output = new InspectionSpecification()
            {
                InspectionLotNumber = lotNumber,
                NodeNumber = nodeNumber,
                CharacteristicNumber = charNumber,
                TargetValue = target,
                UpperSpecificationLimit = upperLimit,
                LowerSpecificationLimit = lowerLimit,
                UM = data[6]
            };

            output.InspectionCharacteristic = new InspectionCharacteristic()
            {
                Name = data[7]
            };

            return output;
        }

        #endregion Methods
    }
}