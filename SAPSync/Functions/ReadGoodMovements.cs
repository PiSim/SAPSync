using SSMD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Functions
{
    public class ReadGoodMovements : ReadTableBase<GoodMovement>
    {
        public ReadGoodMovements()
        {
            _tableName = "MSEG";

            _fields = new string[]
                {
                    "AUFNR",
                    "MATNR",
                    "MENGE",
                    "MEINS"
                };
        }


        internal override GoodMovement ConvertDataArray(string[] data)
        {

            if (!int.TryParse(data[0], out int orderNumber) 
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double movementQuantity) )
                return null;

            GoodMovement output = new GoodMovement()
            {
                OrderNumber  = orderNumber,
                Material = new Material()
                {
                    Code = data[1]
                },

                Quantity = movementQuantity,
                UM = data[3]
                               
            };

            return output;

        }
    }
}
