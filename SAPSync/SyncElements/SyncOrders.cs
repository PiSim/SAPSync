using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class SyncOrders : SyncElement<Order>
    {
        #region Fields

        private IDictionary<string, Material> _materialDictionary;
        private IDictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Constructors

        public SyncOrders()
        {
            Name = "Ordini";
            _missingMaterials = new List<string>();
        }

        #endregion Constructors

        #region Methods

        private readonly List<string> _missingMaterials;

        protected override void Initialize()
        {
            base.Initialize();
            _materialDictionary = _sSMDData.RunQuery(new MaterialsQuery()).ToDictionary(mat => mat.Code, mat => mat);

            if (_materialDictionary == null)
                throw new ArgumentNullException("MaterialDictionary");

            _orderDictionary = _sSMDData.RunQuery(new OrdersQuery()).ToDictionary(ord => ord.Number, ord => ord);

            if (_orderDictionary == null)
                throw new ArgumentNullException("Order Dictionary");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();
            IRfcTable recordTable = RetrieveOrders(_rfcDestination);
            _recordsToInsert = ConvertOrdersTable(recordTable);
        }

        private IList<Order> ConvertOrdersTable(IRfcTable ordersTable)
        {
            IList<Order> output = new List<Order>();

            foreach (IRfcStructure row in ordersTable)
            {
                int currentOrderNumber;
                string orderstring = row.GetString("order_number");

                if (int.TryParse(orderstring, out currentOrderNumber))
                {
                    if (_orderDictionary.ContainsKey(currentOrderNumber))
                        continue;

                    string materialCode = row.GetString("material");
                    int materialID;

                    if (_materialDictionary.ContainsKey(materialCode))
                        materialID = _materialDictionary[materialCode].ID;
                    else
                    {
                        _missingMaterials.Add(row.GetString("material"));
                        continue;
                    }

                    Order newOrder = new Order()
                    {
                        Number = currentOrderNumber,
                        MaterialID = materialID,
                        OrderType = row.GetString("order_type")
                    };

                    output.Add(newOrder);
                };
            }
            return output;
        }

        private IRfcTable RetrieveOrders(RfcDestination destination)
        {
            IRfcTable output;

            try
            {
                output = new OrdersGetList().Invoke(destination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveOrders error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}