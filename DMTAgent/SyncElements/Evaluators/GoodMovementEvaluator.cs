﻿using DMTAgent.SyncElements.Validators;
using SSMD;
using System;

namespace DMTAgent.SyncElements.Evaluators
{
    public class GoodMovementEvaluator : RecordEvaluator<GoodMovement, Tuple<long, int>>
    {
        #region Constructors

        public GoodMovementEvaluator(RecordEvaluatorConfiguration configuration = null) : base(configuration)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Tuple<long, int> GetIndexKey(GoodMovement record) => record.GetIndexKey();

        protected override IRecordValidator<GoodMovement> GetRecordValidator() => new GoodMovementValidator();

        #endregion Methods
    }
}