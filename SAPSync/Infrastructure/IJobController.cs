using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IJobController
    {
        #region Events

        event EventHandler JobCompleted;

        #endregion Events

        Task GetAwaiterForActiveOperations();
        IJob StartJob(IEnumerable<ISyncElement> syncElements);
        event EventHandler NewJobStarted;

        event EventHandler JobStarting;
        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
                
        ICollection<IJob> ActiveJobs { get; }
        ICollection<IJob> CompletedJobs { get; }
    }
}
