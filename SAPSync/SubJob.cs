using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SubJob : ISubJob
    {
        public SubJob(ISyncElement parentElement)
        {
            Status = JobStatus.OnQueue;
        }

        public Task CurrentTask { get; protected set; }

        public JobStatus Status { get; protected set; }

        public IDictionary<Type, object> Resources { get; }

        public ICollection<ISubJob> Dependencies { get; }

        public ISyncElement SyncElement { get; }

        public ICollection<ISyncOperation> Operations { get; }

        public event EventHandler SubJobStarted;
        public event EventHandler SubJobCompleted;
        public event EventHandler StatusChanged;

        public void CheckStatus()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public async void StartAsync() => await Task.Run(() => Start());
    }
}
