using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync
{
    public class Job : JobBase, IJob
    {
        public Job(IEnumerable<ISyncElement> syncElements)
        {
            Status = JobStatus.OnQueue;
            SyncElementsStack = new List<ISyncElement>(syncElements);

            foreach (ISyncElement syncElement in SyncElementsStack)
                SubJobs.Add(new SubJob(syncElement));

            foreach (ISubJob subJob in SubJobs)
                foreach (ISyncElement dependency in subJob.SyncElement.Dependencies)
                {
                    ISubJob dependencyJob = SubJobs.FirstOrDefault(sjb => sjb.SyncElement == dependency);
                    if (dependencyJob != null)
                        subJob.Dependencies.Add(dependencyJob);
                }
        }
        
        public override void Start()
        {
            RaiseOnStarting();
            ExecuteAsync();
            RaiseOnStarted();
        }

        protected virtual async void ExecuteAsync()
        {
            int remainingSubJobs = SubJobs.Where(sjb => sjb.Status == JobStatus.OnQueue || sjb.Status == JobStatus.Ready ).Count();
            while (remainingSubJobs > 0)
            {
                await new Task(() => CycleSubJobs());
                int newRemainingSubJobs = SubJobs.Where(sjb => sjb.Status == JobStatus.OnQueue || sjb.Status == JobStatus.Ready).Count();

                if (newRemainingSubJobs == remainingSubJobs)
                    throw new Exception("Infinite loop. Check for circular dependencies");

                remainingSubJobs = newRemainingSubJobs;
            }
        }

        protected virtual void CycleSubJobs()
        {
            CheckSubJobReadiness();
            StartReadySubJobs();
        }

        protected virtual void CheckSubJobReadiness()
        {
            foreach (ISubJob subJob in SubJobs.Where(sjb => sjb.Status == JobStatus.OnQueue))
                subJob.CheckStatus();
        }


        protected virtual void StartReadySubJobs()
        {
            foreach (ISubJob subJob in SubJobs.Where(sjb => sjb.Status == JobStatus.Ready))
                subJob.StartAsync();
        }

        public ICollection<ISyncElement> SyncElementsStack { get; }        
        public ICollection<ISubJob> SubJobs { get; }        
    }
}
