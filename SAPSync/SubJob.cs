using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SubJob : JobBase, ISubJob
    {
        public SubJob(ISyncElement targetElement)
        {
            Status = JobStatus.OnQueue;
            TargetElement = targetElement;
        }
        
        public IDictionary<Type, object> Resources { get; }

        public ICollection<ISubJob> Dependencies { get; }

        public ISyncElement TargetElement { get; }

        public void CheckStatus()
        {
            if (Status == JobStatus.OnQueue && Dependencies.All(dep => dep.Status == JobStatus.Completed))
                ChangeStatus(JobStatus.Ready);
        }

        public void Complete(bool isSuccesful = true)
        {
            if (isSuccesful)
                ChangeStatus(JobStatus.Completed);
            else
                ChangeStatus(JobStatus.Failed);

            RaiseOnCompleted();
        }

        public override void Start()
        {
            if (Status != JobStatus.Ready)
                throw new InvalidOperationException("Job is not in Ready state");

            ChangeStatus(JobStatus.Running);
            TargetElement.Execute(this);
        }
               
    }
}
