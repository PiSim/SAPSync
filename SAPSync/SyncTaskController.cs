using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncTaskController : IJobController
    {

        #region Events

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
        public event EventHandler NewJobStarted;

        public event EventHandler JobCompleted;
        public event EventHandler JobStarting;

        #endregion Events

        public SyncTaskController()
        {
            ActiveJobs = new HashSet<IJob>();
            CompletedJobs = new HashSet<IJob>();
        }

        public ICollection<IJob> ActiveJobs { get; }
        public ICollection<IJob> CompletedJobs { get; }

        public Task GetAwaiterForOpenReadTasks()
        {
            return Task.WhenAll(ActiveJobs.SelectMany(sts => sts.ActiveReadTasks).ToList());
        }

        public void StartJob(IJob task)
        {
            task.JobCompleted += OnSyncTaskCompleted;
            task.SyncErrorRaised += OnSyncErrorRaised;
            task.JobStarting += OnSyncTaskStarting;
            task.JobStarted += OnSyncTaskStarted;
            ActiveJobs.Add(task);
            task.Start();
            
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            SyncErrorRaised?.Invoke(sender, e);
        }

        protected virtual void OnSyncTaskCompleted(object sender, EventArgs e)
        {
            SyncTask completedTask = sender as SyncTask;
            if (ActiveJobs.Contains(completedTask))
                ActiveJobs.Remove(completedTask);
            CompletedJobs.Add(completedTask);
            JobCompleted?.Invoke(sender, e);
        }

        protected virtual void OnSyncTaskStarting(object sender, EventArgs e) => JobStarting?.Invoke(sender, e);
        

        protected virtual void OnSyncTaskStarted(object sender, EventArgs e) => NewJobStarted?.Invoke(sender, e);
    }
}
