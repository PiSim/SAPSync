using DataAccessCore;
using DMTAgent.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;

namespace DMTAgent.SyncElements
{
    public class OperationAggregator : SyncElementBase
    {
        #region Constructors

        public OperationAggregator(IDataService<SSMDContext> dataService , string name = "", SyncElementConfiguration configuration = null) : base(dataService, name, configuration)
        {

        }

        #endregion Constructors

        #region Properties

        public ICollection<ISyncOperation> Operations { get; } = new List<ISyncOperation>();

        protected IEnumerator<ISyncOperation> OperationEnumerator { get; set; }

        #endregion Properties

        #region Methods

        public virtual ISyncElement DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                Dependencies.Add(element);

            return this;
        }

        public override void Execute(ISubJob newJob)
        {
            base.Execute(newJob);
            OperationEnumerator = Operations.GetEnumerator();
            StartNextOperation();
        }

        public virtual OperationAggregator HasOperation(ISyncOperation newOperation)
        {
            Operations.Add(newOperation);
            newOperation.SetParent(this);
            newOperation.OperationCompleted += OnOperationCompleted;
            return this;
        }

        protected virtual void StartNextOperation()
        {
            OperationEnumerator.MoveNext();
            if (OperationEnumerator.Current == null)
                FinalizeSync();
            else
                OperationEnumerator.Current.StartAsync(CurrentJob);
        }

        private void OnOperationCompleted(object sender, EventArgs e) => StartNextOperation();

        #endregion Methods
    }
}