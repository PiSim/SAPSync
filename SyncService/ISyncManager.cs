using System;
using System.Threading.Tasks;

namespace SyncService
{
    public interface ISyncManager
    {
        #region Events

        event EventHandler SyncTaskCompleted;

        #endregion Events

        #region Methods

        Task GetAwaiterForOpenReadTasks();

        DateTime? GetTimeForNextUpdate();

        void StartSync();

        #endregion Methods
    }
}