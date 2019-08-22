using SSMD;

namespace SAPSync.RFCFunctions
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

        internal override MaterialFamilyLevel ConvertDataArray(string[] data)
        {
            MaterialFamilyLevel output = new MaterialFamilyLevel();

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
                Code = code,
                Description = data[1]
            };

            return output;
        }

        #endregion Methods
    }
}