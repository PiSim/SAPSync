using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync
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

        ICollection<IJob> ActiveJobs { get; }

        ICollection<IJob> CompletedJobs { get; }

        #endregion Properties

        #region Methods

        Task GetAwaiterForActiveOperations();

        IJob StartJob(IEnumerable<ISyncElement> syncElements);

        #endregion Methods
    }
}