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
        public SubJob(ISyncElement parentElement)
        {
            Status = JobStatus.OnQueue;
            SyncElement = parentElement;
        }


        public IDictionary<Type, object> Resources { get; }

        public ICollection<ISubJob> Dependencies { get; }

        public ISyncElement SyncElement { get; }

        public ICollection<ISyncOperation> Operations { get; }
        
        public void CheckStatus()
        {
            if (Status == JobStatus.OnQueue && Dependencies.All(dep => dep.Status == JobStatus.Completed))
                ChangeStatus(JobStatus.Ready);
        }

        public override void Start()
        {
            
        }
    }
}
