using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncInspectionLots : SyncElement<InspectionLot>
    {
        #region Fields

        private IDictionary<long, InspectionLot> _inspectionLotDictionary;
        private IDictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Constructors

        public SyncInspectionLots()
        {
            Name = "Lotti di Controllo";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            _orderDictionary = _sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);

            if (_orderDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Ordini");

            _inspectionLotDictionary = _sSMDData.RunQuery(new InspectionLotsQuery()).ToDictionary(ispl => ispl.Number, ispl => ispl);

            if (_inspectionLotDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Lotti");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();
            IRfcTable lotsTable = RetrieveInspectionLots(_rfcDestination);

            IList<InspectionLot> inspectionLots = ConvertInspectionLotTable(lotsTable);

            _recordsToInsert = GetValidatedLots(inspectionLots);
        }

        private List<InspectionLot> ConvertInspectionLotTable(IRfcTable materialTable)
        {
            List<InspectionLot> output = new List<InspectionLot>();

            foreach (IRfcStructure row in materialTable)
            {
                long currentLotNumber;
                string lotNumberString = row.GetString("insplot");

                if (!long.TryParse(lotNumberString, out currentLotNumber))
                    continue;

                if (_inspectionLotDictionary.ContainsKey(currentLotNumber))
                    continue;

                InspectionLot newInspectionLot = new InspectionLot();
                newInspectionLot.Number = currentLotNumber;

                string orderNumberString = row.GetString("order_no");

                int orderNumber;
                if (!int.TryParse(orderNumberString, out orderNumber))
                    continue;

                newInspectionLot.OrderNumber = orderNumber;

                output.Add(newInspectionLot);
            }

            return output;
        }

        private IList<InspectionLot> GetValidatedLots(IEnumerable<InspectionLot> inspectionLots)
        {
            IList<InspectionLot> output = new List<InspectionLot>();

            foreach (InspectionLot currentLot in inspectionLots)
            {
                if (!_orderDictionary.ContainsKey(currentLot.OrderNumber) ||
                    _inspectionLotDictionary.ContainsKey(currentLot.Number))
                    continue;

                output.Add(currentLot);
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