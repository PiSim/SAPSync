using SAP.Middleware.Connector;
using SSMD;

namespace SAPSync.RFCFunctions
{
    public class ReadInspectionCharacteristics : ReadTableBase<InspectionCharacteristic>
    {
        public override string Name => "ReadInspectionCharacteristics";
        #region Constructors

        public ReadInspectionCharacteristics()
        {
            _tableName = "QPMK";
            _fields = new string[]
            {
                "MKMNR",
                "MASSEINHSW",
                "VERSION",
                "ZAEHLER"
            };
        }

        #endregion Constructors

        #region Methods

        internal override InspectionCharacteristic ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);

            InspectionCharacteristic converted = new InspectionCharacteristic()
            {
                Name = data[0],
                UM = data[1]
            };

            return converted;
        }

        #endregion Methods
    }
}