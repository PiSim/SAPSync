using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync
{
    public class JobController : IJobController
    {
        #region Constructors

        public JobController()
        {
            ActiveJobs = new HashSet<IJob>();
            CompletedJobs = new HashSet<IJob>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler JobCompleted;

        public event EventHandler JobStarting;

        public event EventHandler NewJobStarted;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        public ICollection<IJob> ActiveJobs { get; }
        public ICollection<IJob> CompletedJobs { get; }

        #endregion Properties

        #region Methods

        public Task GetAwaiterForActiveOperations() => Task.WhenAll(
            ActiveJobs.SelectMany(sts => sts.SubJobs)
                .Select(sjb => sjb.CurrentTask)
                .ToList());

        public IJob StartJob(IEnumerable<ISyncElement> syncElements)
        {
            Job newJob = new Job(syncElements);
            newJob.OnCompleted += OnJobCompleted;
            newJob.SyncErrorRaised += OnSyncErrorRaised;
            newJob.OnStarting += OnJobStarting;
            newJob.OnStarted += OnJobStarted;
            ActiveJobs.Add(newJob);
            newJob.StartAsync();
            return newJob;
        }

        protected virtual void OnJobCompleted(object sender, EventArgs e)
        {
            Job completedTask = sender as Job;
            if (ActiveJobs.Contains(completedTask))
                ActiveJobs.Remove(completedTask);
            CompletedJobs.Add(completedTask);
            JobCompleted?.Invoke(sender, e);
        }

        protected virtual void OnJobStarted(object sender, EventArgs e) => NewJobStarted?.Invoke(sender, e);

        protected virtual void OnJobStarting(object sender, EventArgs e) => JobStarting?.Invoke(sender, e);

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            SyncErrorRaised?.Invoke(sender, e);
        }

        #endregion Methods
    }
}