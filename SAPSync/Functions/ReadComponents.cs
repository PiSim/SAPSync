using SAP.Middleware.Connector;
using SSMD;
using System.Collections.Generic;

namespace SAPSync.Functions
{
    public class ReadComponents : ReadTableBase<Component>
    {
        #region Constructors

        public ReadComponents() : base()
        {
            _tableName = "MARA";
            _fields = new string[]
            {
                "MATNR"
            };

            _selectionOptions = new List<string>() { "MATNR LIKE 'P%' OR MATNR LIKE 'S%'" };
        }

        #endregion Constructors

        #region Methods

        internal override Component ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);

            string code = data[0].Trim();

            Component output = new Component()
            {
                Name = code
            };
            return output;
        }

        #endregion Methods
    }
}