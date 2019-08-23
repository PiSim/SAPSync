using SAP.Middleware.Connector;
using SSMD;

namespace SAPSync.RFCFunctions
{
    public class ReadMaterialFamilies : ReadTableBase<MaterialFamily>
    {
        #region Constructors

        public ReadMaterialFamilies() : base()
        {
            _tableName = "T179";
            _fields = new string[]
            {
                "PRODH"
            };
        }

        #endregion Constructors

        #region Methods

        internal override MaterialFamily ConvertRow(IRfcStructure row)
        {
            MaterialFamily output = new MaterialFamily();

            string[] data = row.GetString("WA").Split(_separator);

            string fullcode = data[0];

            if (fullcode.Length != 18)
                return null;

            string l1code = fullcode.Substring(0, 6),
                l2code = fullcode.Substring(6, 6),
                l3code = fullcode.Substring(12, 6);

            MaterialFamilyLevel l1 = new MaterialFamilyLevel()
            {
                Level = 1,
                Code = l1code
            };

            MaterialFamilyLevel l2 = new MaterialFamilyLevel()
            {
                Level = 2,
                Code = l2code
            };

            MaterialFamilyLevel l3 = new MaterialFamilyLevel()
            {
                Level = 3,
                Code = l3code
            };

            output = new MaterialFamily()
            {
                L1 = l1,
                L2 = l2,
                L3 = l3
            };

            return output;
        }

        #endregion Methods
    }
}