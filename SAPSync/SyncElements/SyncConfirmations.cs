using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncConfirmations : SyncElement<OrderConfirmation>
    {
        #region Fields

        private IDictionary<Tuple<int, int>, OrderConfirmation> _confirmationDictionary;
        private IDictionary<int, Order> _orderDictionary;

        #endregion Fields

        #region Constructors

        public SyncConfirmations()
        {
            Name = "Conferme";
        }

        #endregion Constructors

        #region Methods

        public IList<OrderConfirmation> InvalidConfirmations { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            _orderDictionary = _sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);

            if (_orderDictionary == null)
                throw new ArgumentNullException("OrderDictionary");

            if (_orderDictionary.Keys.Count == 0)
                return;

            _confirmationDictionary = _sSMDData.RunQuery(new ConfirmationsQuery()).ToDictionary(oc => new Tuple<int, int>(oc.ConfirmationNumber, oc.ConfirmationCounter), oc => oc);

            if (_confirmationDictionary == null)
                throw new Exception("Impossibile recuperare dizionario conferme");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();

            IList<OrderConfirmation> confirmationsTable = RetrieveConfirmations();

            IEnumerable<OrderConfirmation> validConfirmations = GetValidatedConfirmations(confirmationsTable);
            foreach (OrderConfirmation oc in validConfirmations)
            {
                Tuple<int, int> currentKey = new Tuple<int, int>(oc.ConfirmationNumber, oc.ConfirmationCounter);

                if (_confirmationDictionary.ContainsKey(currentKey))
                    _recordsToUpdate.Add(oc);
                else
                    _recordsToInsert.Add(oc);
            }
        }

        private IEnumerable<OrderConfirmation> GetValidatedConfirmations(IEnumerable<OrderConfirmation> confirmations)
        {
            InvalidConfirmations = new List<OrderConfirmation>();
            List<OrderConfirmation> output = new List<OrderConfirmation>();

            foreach (OrderConfirmation newConfirmation in confirmations)
            {
                if (_orderDictionary.ContainsKey(newConfirmation.OrderNumber))
                    output.Add(newConfirmation);
                else
                    InvalidConfirmations.Add(newConfirmation);
            }
            return output;
        }

        private IList<OrderConfirmation> RetrieveConfirmations()
        {
            IList<OrderConfirmation> output;

            try
            {
                output = new ReadConfirmations().Invoke(_rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveConfirmations error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}