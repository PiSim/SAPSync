using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class InspectionLotEvaluator : RecordEvaluator<InspectionLot, long>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new InspectionLotValidator();
        }

        protected override long GetIndexKey(InspectionLot record) => record.Number;

        #endregion Methods
    }

    public class InspectionLotValidator : IRecordValidator<InspectionLot>
    {
        #region Fields

        private IDictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _orderDictionary != null;

        public InspectionLot GetInsertableRecord(InspectionLot record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderDictionary = sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);
        }

        public bool IsValid(InspectionLot record) => _orderDictionary.ContainsKey(record.OrderNumber);

        #endregion Methods
    }

    public class SyncInspectionLots : SyncSAPTable<InspectionLot>
    {
        #region Constructors

        public SyncInspectionLots(RfcDestination rfcDestination, SSMDData sSMDData) : base(rfcDestination, sSMDData)
        {
            Name = "Lotti di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new InspectionLotEvaluator() { IgnoreExistingRecords = true };
        }

        protected override IList<InspectionLot> ReadRecordTable()
        {
            IRfcTable lotsTable = RetrieveInspectionLots(_rfcDestination);

            return ConvertInspectionLotTable(lotsTable);
        }

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

        private IRfcTable RetrieveInspectionLots(RfcDestination rfcDestination)
        {
            IRfcTable output;

            try
            {
                output = new InspLotGetList().Invoke(rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveInspectionLots error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}