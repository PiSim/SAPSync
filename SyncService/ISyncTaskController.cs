using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncService
{
    public interface ISyncTaskController
    {
        #region Events

        event EventHandler SyncTaskCompleted;

        #endregion Events

        Task GetAwaiterForOpenReadTasks();
        void RunTask(ISyncTask task);
        event EventHandler NewSyncTaskStarted;

        event EventHandler SyncTaskStarting;
        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        
        ICollection<ISyncTask> ActiveTasks { get; }
        ICollection<ISyncTask> CompletedTasks { get; }
    }
}
