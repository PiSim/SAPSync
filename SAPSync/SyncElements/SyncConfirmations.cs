using DataAccessCore;
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
        private IDictionary<int, WorkCenter> _workCenterDictionary;

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
            _confirmationDictionary = _sSMDData.RunQuery(new ConfirmationsQuery()).ToDictionary(oc => new Tuple<int, int>(oc.ConfirmationNumber, oc.ConfirmationCounter), oc => oc);
            _workCenterDictionary = _sSMDData.RunQuery(new Query<WorkCenter, SSMDContext>()).ToDictionary(wc => wc.ID);
        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();

            if (_confirmationDictionary == null)
                throw new Exception("Impossibile recuperare dizionario conferme");

            if (_workCenterDictionary == null)
                throw new Exception("Impossibile recuperare dizionario Centri di Lavoro");

            if (_orderDictionary == null)
                throw new ArgumentNullException("Impossibile recuperare dizionario Ordini");

            if (_orderDictionary.Keys.Count == 0)
                Abort("La lista ordini non contiene elementi");
        }

        protected override bool MustIgnoreRecord(OrderConfirmation record) => !_orderDictionary.ContainsKey(record.OrderNumber)
            || !_workCenterDictionary.ContainsKey(record.WorkCenterID);

        protected override bool IsNewRecord(OrderConfirmation record)
        {
            Tuple<int, int> currentKey = new Tuple<int, int>(record.ConfirmationNumber, record.ConfirmationCounter);
            return !_confirmationDictionary.ContainsKey(currentKey);
        }        

        protected override IList<OrderConfirmation> ReadRecordTable() =>  new ReadConfirmations().Invoke(_rfcDestination);

        #endregion Methods
    }
}