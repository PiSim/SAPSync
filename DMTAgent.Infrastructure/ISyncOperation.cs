using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMTAgent.Infrastructure
{
    public interface ISyncOperation
    {
        #region Events

        event EventHandler OperationCompleted;

        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        ICollection<Task> ChildrenTasks { get; }
        ISubJob CurrentJob { get; }
        string Name { get; }

        #endregion Properties

        #region Methods

        void SetParent(ISyncElement syncElement);

        void Start(ISubJob newJob);

        void StartAsync(ISubJob newJob);

        #endregion Methods
    }
}