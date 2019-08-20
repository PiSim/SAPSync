using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncTaskController : ISyncTaskController
    {

        #region Events

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
        public event EventHandler NewSyncTaskStarted;

        public event EventHandler SyncTaskCompleted;
        public event EventHandler SyncTaskStarting;

        #endregion Events

        public SyncTaskController()
        {
            ActiveTasks = new HashSet<ISyncTask>();
            CompletedTasks = new HashSet<ISyncTask>();
        }

        public ICollection<ISyncTask> ActiveTasks { get; }
        public ICollection<ISyncTask> CompletedTasks { get; }

        public Task GetAwaiterForOpenReadTasks()
        {
            return Task.WhenAll(ActiveTasks.SelectMany(sts => sts.ActiveReadTasks).ToList());
        }

        public void RunTask(ISyncTask task)
        {
            task.SyncTaskCompleted += OnSyncTaskCompleted;
            task.SyncErrorRaised += OnSyncErrorRaised;
            task.SyncTaskStarting += OnSyncTaskStarting;
            task.SyncTaskStarted += OnSyncTaskStarted;
            ActiveTasks.Add(task);
            task.Start();
            
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            SyncErrorRaised?.Invoke(sender, e);
        }

        protected virtual void OnSyncTaskCompleted(object sender, EventArgs e)
        {
            SyncTask completedTask = sender as SyncTask;
            if (ActiveTasks.Contains(completedTask))
                ActiveTasks.Remove(completedTask);
            CompletedTasks.Add(completedTask);
            SyncTaskCompleted?.Invoke(sender, e);
        }

        protected virtual void OnSyncTaskStarting(object sender, EventArgs e) => SyncTaskStarting?.Invoke(sender, e);
        

        protected virtual void OnSyncTaskStarted(object sender, EventArgs e) => NewSyncTaskStarted?.Invoke(sender, e);
    }
}
