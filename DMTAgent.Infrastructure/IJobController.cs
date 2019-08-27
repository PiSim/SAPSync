using DMTAgent.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMTAgent.Infrastructure
{ 
    public interface IJobController
    {
        #region Events

        event EventHandler JobCompleted;

        event EventHandler JobStarting;

        event EventHandler NewJobStarted;

        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        ICollection<IJob> Jobs { get; }

        #endregion Properties

        #region Methods

        Task GetAwaiterForActiveOperations();

        ICollection<IJob> GetJobs(bool includeCompleted = false);

        IJob StartJob(IEnumerable<ISyncElement> syncElements);

        #endregion Methods
    }
}