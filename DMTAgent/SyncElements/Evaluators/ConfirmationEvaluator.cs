﻿using DataAccessCore;
using DMTAgent.SyncElements.Validators;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent.SyncElements
{
    public class ConfirmationEvaluator : RecordEvaluator<OrderConfirmation, Tuple<int, int>>
    {
        #region Constructors

        public ConfirmationEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Tuple<int, int> GetIndexKey(OrderConfirmation record) => new Tuple<int, int>(record.ConfirmationNumber, record.ConfirmationCounter);

        protected override IRecordValidator<OrderConfirmation> GetRecordValidator() => new ConfirmationValidator();

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

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
            _orderDictionary = sSMDData.RunQuery(new OrdersQuery()).ToDictionary(order => order.Number, order => order);
            _workCenterDictionary = sSMDData.RunQuery(new Query<WorkCenter, SSMDContext>()).ToDictionary(wc => wc.ID);
        }

        public bool IsValid(OrderConfirmation record) => _orderDictionary.ContainsKey(record.OrderNumber) && _workCenterDictionary.ContainsKey(record.WorkCenterID);

        #endregion Methods
    }
}