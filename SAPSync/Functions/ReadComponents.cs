using SSMD;
using System.Collections.Generic;

namespace SAPSync.Functions
{
    public class ReadComponents : ReadTableBase<Component>
    {
        #region Constructors

        public ReadComponents() : base()
        {
            _tableName = "MAKT";
            _fields = new string[]
            {
                "MATNR",
                "MAKTX"
            };

            _selectionOptions = new List<string>() { "SPRAS = 'IT' AND (MATNR LIKE 'P%' OR MATNR LIKE 'S%')" };
        }

        #endregion Constructors

        #region Methods

        internal override Component ConvertDataArray(string[] data)
        {
            string code = data[0].Trim();

            Component output = new Component()
            {
                Name = code,
                Description = data[1]
            };
            return output;
        }

        #endregion Methods
    }
}