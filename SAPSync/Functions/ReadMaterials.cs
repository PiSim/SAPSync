using SAP.Middleware.Connector;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.Functions
{
    public class ReadMaterials : ReadTableBase<Material>
    {
        #region Constructors

        public ReadMaterials()
        {
            _tableName = "MARA";
            _fields = new string[]
            {
                "MATNR",
                "PRDHA"
            };

            _selectionOptions = new List<string> { "MATNR LIKE '1%' OR MATNR LIKE '2%' OR MATNR LIKE '3%'" };
        }

        #endregion Constructors

        #region Methods

        internal override Material ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);

            string code = data[0].Trim();
            MaterialFamily tempFamily;
            if (data[1].Length != 18)
                tempFamily = null;
            else
                tempFamily = new MaterialFamily()
                {
                    L1 = new MaterialFamilyLevel() { Level = 1, Code = data[1].Substring(0, 6) },
                    L2 = new MaterialFamilyLevel() { Level = 2, Code = data[1].Substring(6, 6) },
                    L3 = new MaterialFamilyLevel() { Level = 3, Code = data[1].Substring(12, 6) }
                };

            Material output = new Material()
            {
                Code = code,
                MaterialFamily = tempFamily
            };
            return output;
        }

        #endregion Methods
    }
}