using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements
{
    public class OperationAggregator : SyncElementBase
    {

        public OperationAggregator(string name = "", SyncElementConfiguration configuration = null) : base(name, configuration)
        {
        }

        #region Methods

        public virtual OperationAggregator HasOperation(ISyncOperation newOperation)
        {
            Operations.Add(newOperation);
            newOperation.SetParent(this);
            return this;
        }

        public virtual ISyncElement DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                Dependencies.Add(element);

            return this;
        }


        public ICollection<ISyncOperation> Operations { get; } = new List<ISyncOperation>();


        public override void Execute(ISubJob newJob)
        {
            base.Execute(newJob);
            OperationEnumerator = Operations.GetEnumerator();
            StartNextOperation();
        }

        protected IEnumerator<ISyncOperation> OperationEnumerator { get; set; }

        protected virtual void StartNextOperation()
        {
            if (OperationEnumerator.Current == null)
                FinalizeSync();

            else
            {
                OperationEnumerator.Current.StartAsync();
                OperationEnumerator.MoveNext();
            }
        }

        #endregion Methods

    }
}