﻿using DataAccessCore;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class ConfirmationEvaluator : RecordEvaluator<OrderConfirmation, Tuple<int, int>>
    {
        public ConfirmationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {

        }
        #region Methods

        protected override IRecordValidator<OrderConfirmation> GetRecordValidator() => new ConfirmationValidator();
        

        protected override Tuple<int, int> GetIndexKey(OrderConfirmation record) => new Tuple<int, int>(record.ConfirmationNumber, record.ConfirmationCounter);

        #endregion Methods
    }

    public class ConfirmationValidator : IRecordValidator<OrderConfirmation>
    {
        #region Fields

        private IDictionary<int, Order> _orderDictionary;
        private IDictionary<int, WorkCenter> _workCenterDictionary;

        #endregion Fields

        #region Methods

        public bool CheckIndexesInitialized() => (_orderDictionary == null || _workCenterDictionary == null);

        public OrderConfirmation GetInsertableRecord(OrderConfirmation record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            _orderDictionary = sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);
            _workCenterDictionary = sSMDData.RunQuery(new Query<WorkCenter, SSMDContext>()).ToDictionary(wc => wc.ID);
        }

        public bool IsValid(OrderConfirmation record) => _orderDictionary.ContainsKey(record.OrderNumber) && _workCenterDictionary.ContainsKey(record.WorkCenterID);

        #endregion Methods
    }

}