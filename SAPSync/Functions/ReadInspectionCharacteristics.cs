using SAP.Middleware.Connector;
using SSMD;

namespace SAPSync.Functions
{
    public class ReadInspectionCharacteristics : ReadTableBase<InspectionCharacteristic>
    {
        #region Constructors

        public ReadInspectionCharacteristics()
        {
            _tableName = "QPMK";
            _fields = new string[]
            {
                "MKMNR",
                "MASSEINHSW"
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