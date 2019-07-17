using SAP.Middleware.Connector;
using SSMD;

namespace SAPSync.Functions
{
    public class ReadOrderComponents : ReadTableBase<OrderComponent>
    {
        public override string Name => "ReadOrderComponents";
        #region Constructors

        public ReadOrderComponents()
        {
            _tableName = "RESB";

            _fields = new string[]
            {
                "AUFNR",
                "MATNR"
            };
        }

        #endregion Constructors

        #region Methods

        internal override OrderComponent ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);

            if (!int.TryParse(data[0], out int orderNumber))
                return null;

            string matcode = data[1];

            OrderComponent output = new OrderComponent()
            {
                OrderNumber = orderNumber,
                Component = new Component()
                {
                    Name = matcode
                }
            };
            return output;
        }

        protected override void ConfigureBatchingOptions()
        {
            base.ConfigureBatchingOptions();

            BatchingOptions = new ReadTableBatchingOptions()
            {
                Field = "AUFNR",
                StringFormat = "000000000000",
                MinValue = 1000000,
                MaxValue = 1999999,
                BatchSize = 10000
            };
        }

        #endregion Methods
    }
}