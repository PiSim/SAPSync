using System.Collections.Generic;
using SAP.Middleware.Connector;
using SAPSync.SyncElements;
using SSMD;

namespace SAPSync.RFCFunctions
{
    public class InspLotGetList : BAPITableFunctionBase, IRecordReader<InspectionLot>
    {
        #region Constructors

        public InspLotGetList() : base()
        {
            _functionName = "BAPI_INSPLOT_GETLIST";
            _tableName = "insplot_LIST";
        }

        public IEnumerable<InspectionLot> ReadRecords() => ConvertInspectionLotTable(Invoke(new SAPReader().GetRfcDestination()));
        
        private List<InspectionLot> ConvertInspectionLotTable(IRfcTable materialTable)
        {
            List<InspectionLot> output = new List<InspectionLot>();

            foreach (IRfcStructure row in materialTable)
            {
                string lotNumberString = row.GetString("insplot");

                if (!long.TryParse(lotNumberString, out long currentLotNumber))
                    continue;

                InspectionLot newInspectionLot = new InspectionLot
                {
                    Number = currentLotNumber
                };

                string orderNumberString = row.GetString("order_no");

                if (!int.TryParse(orderNumberString, out int orderNumber))
                    continue;

                newInspectionLot.OrderNumber = orderNumber;

                output.Add(newInspectionLot);
            }

            return output;
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("max_rows", 99999999);
        }

        #endregion Methods
    }
}