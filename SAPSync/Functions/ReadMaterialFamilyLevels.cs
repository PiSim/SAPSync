using SAP.Middleware.Connector;
using SSMD;

namespace SAPSync.Functions
{
    public class ReadMaterialFamilyLevels : ReadTableBase<MaterialFamilyLevel>
    {
        #region Constructors

        public ReadMaterialFamilyLevels()
        {
            _tableName = "T179T";
            _fields = new string[]
            {
                "PRODH",
                "VTEXT"
            };
        }

        #endregion Constructors

        #region Methods

        internal override MaterialFamilyLevel ConvertRow(IRfcStructure row)
        {
            MaterialFamilyLevel output = new MaterialFamilyLevel();

            string[] data = row.GetString("WA").Split(_separator);

            string fullcode = data[0].Trim();

            if (fullcode == null || fullcode.Length % 6 != 0)
                return null;

            int level;
            string code;

            level = fullcode.Length / 6;
            code = fullcode.Substring((level - 1) * 6, 6);

            output = new MaterialFamilyLevel()
            {
                Level = level,
                Code = code
            };

            return output;
        }

        #endregion Methods
    }
}