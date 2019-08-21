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
    public class Job : SyncElementBase, IJob
    {
        public event EventHandler JobCompleted;
        public event EventHandler JobStarting;
        public event EventHandler JobStarted;

        public Job(IEnumerable<ISyncElement> syncElements)
        {
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
        
        public void Start()
        {
            RaiseJobStarting();
            ExecuteAsync();
            RaiseJobStarted();
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

        protected virtual void OnSyncElementStarting(object sender, EventArgs e)
        {

        }        

        protected virtual void RaiseJobStarted() => JobStarted?.Invoke(this, new EventArgs());
        
        protected virtual void RaiseJobStarting() => JobStarting?.Invoke(this, new EventArgs());
        
        protected virtual void RaiseJobCompleted() => JobCompleted?.Invoke(this, new EventArgs());
        

        public override string Name => "Job";

        public Task CurrentTask { get; protected set; }

        public ICollection<ISubJob> SubJobs { get; }

        public JobStatus Status { get; protected set; }
        
        public async void StartAsync() => await Task.Run(() => Start());

        protected virtual void SyncFailure(Exception e = null)
        {
            RaiseSyncFailed(e);
            ChangeStatus(JobStatus.Failed);
        }

    }
}
