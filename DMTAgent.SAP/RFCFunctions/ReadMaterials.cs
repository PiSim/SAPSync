﻿using SSMD;
using System.Collections.Generic;

namespace DMTAgent.SAP
{
    public class ReadMaterials : ReadTableBase<Material>
    {
        #region Constructors

        public ReadMaterials() : base()
        {
            _tableName = "MARA";
            _fields = new string[]
            {
                "MATNR",
                "PRDHA",
                "ZEINR",
                "AUFNR"
            };

            _selectionOptions = new List<string> { "MATNR LIKE '1%' OR MATNR LIKE '2%' OR MATNR LIKE '3%'" };
        }

        #endregion Constructors

        #region Methods

        internal override Material ConvertDataArray(string[] data)
        {
            string code = data[0].Trim();

            if (!int.TryParse(data[2], out int pdc))
                pdc = 0;

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
                MaterialFamily = tempFamily,
                ControlPlan = pdc,
                Project = new Project() { Code = data[3].Trim() }
            };
            return output;
        }

        #endregion Methods
    }
}