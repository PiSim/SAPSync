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
            Jobs = new HashSet<IJob>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler JobCompleted;

        public event EventHandler JobStarting;

        public event EventHandler NewJobStarted;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        public DateTime EndTime { get; }
        public ICollection<IJob> Jobs { get; }
        public DateTime StartTime { get; }

        #endregion Properties

        #region Methods

        public Task GetAwaiterForActiveOperations() => Task.WhenAll(
            GetJobs()
                .SelectMany(sts => sts.SubJobs)
                .Select(sjb => sjb.CurrentTask)
                .ToList());

        public ICollection<IJob> GetJobs(bool includeCompleted = false)
                    => Jobs.Where(job => includeCompleted || ((job.Status != JobStatus.Completed) || (job.Status != JobStatus.Completed)))
                .ToList();

        public IJob StartJob(IEnumerable<ISyncElement> syncElements)
        {
            Job newJob = new Job(syncElements);
            newJob.OnCompleted += OnJobCompleted;
            newJob.SyncErrorRaised += OnSyncErrorRaised;
            newJob.OnStarting += OnJobStarting;
            newJob.OnStarted += OnJobStarted;
            Jobs.Add(newJob);
            newJob.StartAsync();
            return newJob;
        }

        protected virtual void OnJobCompleted(object sender, EventArgs e)
        {
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