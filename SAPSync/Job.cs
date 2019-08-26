using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync
{
    public class Job : JobBase, IJob
    {
        #region Constructors

        public Job(IEnumerable<ISyncElement> syncElements)
        {
            Status = JobStatus.OnQueue;
            SyncElementsStack = new List<ISyncElement>(syncElements);
            SubJobs = new List<ISubJob>();

            foreach (ISyncElement syncElement in SyncElementsStack)
                SubJobs.Add(new SubJob(syncElement));

            foreach (ISubJob subJob in SubJobs)
            {
                subJob.OnCompleted += OnSubJobCompleted;

                foreach (ISyncElement dependency in subJob.TargetElement.Dependencies)
                {
                    ISubJob dependencyJob = SubJobs.FirstOrDefault(sjb => sjb.TargetElement == dependency);
                    if (dependencyJob != null)
                        subJob.Dependencies.Add(dependencyJob);
                }
            }
        }

        #endregion Constructors

        #region Properties

        public ICollection<ISubJob> SubJobs { get; }

        public ICollection<ISyncElement> SyncElementsStack { get; }

        #endregion Properties

        #region Methods

        public override void Start()
        {
            RaiseOnStarting();
            ExecuteAsync();
            RaiseOnStarted();
        }

        protected virtual void CheckSubJobReadiness()
        {
            foreach (ISubJob subJob in SubJobs.Where(sjb => sjb.Status == JobStatus.OnQueue))
                subJob.CheckStatus();
        }

        protected virtual void CycleSubJobs()
        {
            if (SubJobs.All(sjb => sjb.Status == JobStatus.Completed || sjb.Status == JobStatus.Failed))
                FinalizeJob();
            else
            {
                CheckSubJobReadiness();
                if (!SubJobs.Any(sjb => sjb.Status == JobStatus.Ready))
                    throw new Exception("No ready subjobs, check for circular dependencies.");
                StartReadySubJobs();
            }
        }

        protected virtual async void ExecuteAsync() => await Task.Run(() => CycleSubJobs());

        protected virtual void FinalizeJob()
        {
            RaiseCompleted();
        }

        protected virtual void StartReadySubJobs()
        {
            foreach (ISubJob subJob in SubJobs.Where(sjb => sjb.Status == JobStatus.Ready))
                subJob.StartAsync();
        }

        private void OnSubJobCompleted(object sender, EventArgs e)
        {
            CycleSubJobs();
        }

        #endregion Methods
    }
}