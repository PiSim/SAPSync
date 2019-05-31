using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class OrderEvaluator : RecordEvaluator<Order, int>
    {
        #region Methods

        protected override int GetIndexKey(Order record) => record.Number;

        #endregion Methods
    }

    public class OrderValidator : IRecordValidator<Order>
    {
        #region Fields

        private IDictionary<string, Material> _materialDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => _materialDictionary != null;

        public Order GetInsertableRecord(Order record)
        {
            record.MaterialID = _materialDictionary[record.Material.Code].ID;
            record.Material = null;
            return record;
        }

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _materialDictionary = sSMDData.RunQuery(new MaterialsQuery()).ToDictionary(mat => mat.Code, mat => mat);
        }

        public bool IsValid(Order record) => _materialDictionary.ContainsKey(record.Material.Code);

        #endregion Methods
    }

    public class SyncOrders : SyncElement<Order>
    {
        #region Constructors

        public SyncOrders()
        {
            Name = "Ordini";
        }

        #endregion Constructors

        #region Methods

        protected override void ConfigureRecordEvaluator()
        {
            RecordEvaluator = new OrderEvaluator() { IgnoreExistingRecords = true };
        }

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new OrderValidator();
        }

        protected override IList<Order> ReadRecordTable()
        {
            IRfcTable recordTable = RetrieveOrders(_rfcDestination);
            return ConvertOrdersTable(recordTable);
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
                    string materialCode = row.GetString("material");

                    Order newOrder = new Order()
                    {
                        Number = currentOrderNumber,
                        Material = new Material() { Code = materialCode },
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