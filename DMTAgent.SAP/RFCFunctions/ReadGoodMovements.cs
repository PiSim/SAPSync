using SSMD;
using System.Globalization;

namespace DMTAgent.SAP
{
    public class ReadGoodMovements : ReadTableBase<GoodMovement>
    {
        #region Constructors

        public ReadGoodMovements() : base()
        {
            _tableName = "MSEG";

            _fields = new string[]
                {
                    "AUFNR",
                    "MATNR",
                    "MENGE",
                    "MEINS",
                    "MBLNR",
                    "ZEILE"
                };
        }

        #endregion Constructors

        #region Properties

        public int MaxOdp { get; set; } = 1999999;

        #endregion Properties

        #region Methods

        internal override GoodMovement ConvertDataArray(string[] data)
        {
            if (!long.TryParse(data[4], out long docNumber)
                || !int.TryParse(data[5], out int itemNumber)
                || !int.TryParse(data[0], out int orderNumber)
                || !double.TryParse(data[2], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out double movementQuantity))
                return null;

            GoodMovement output = new GoodMovement()
            {
                DocumentNumber = docNumber,
                ItemNumber = itemNumber,
                OrderNumber = orderNumber,
                Component = new Component()
                {
                    Name = data[1].Trim()
                },

                Quantity = movementQuantity,
                UM = data[3]
            };

            return output;
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

        #endregion Methods
    }
}