﻿using SSMD;
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
        public override string Name => "ReadGoodMovements";
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

        protected override void ConfigureBatchingOptions()
        {
            BatchingOptions = new ReadTableBatchingOptions()
            {
                BatchSize = 10000,
                Field = "AUFNR",
                StringFormat = "000000000000",
                MaxValue = MaxOdp,
                MinValue = 1000000
            };
        }

        public int MaxOdp { get; set; } = 1999999;

        internal override GoodMovement ConvertDataArray(string[] data)
        {

            if (!int.TryParse(data[0], out int orderNumber) 
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double movementQuantity) )
                return null;

            GoodMovement output = new GoodMovement()
            {
                OrderNumber  = orderNumber,
                Component = new Component()
                {
                    Name = data[1].Trim()
                },

                Quantity = movementQuantity,
                UM = data[3]
                               
            };

            return output;

        }
    }
}
