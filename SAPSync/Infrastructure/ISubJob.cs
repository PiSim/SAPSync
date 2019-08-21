using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface ISubJob
    {
        ISyncElement SyncElement { get;}
        ICollection<ISyncOperation> Operations { get; }

        Task CurrentTask { get; }

        void Start();
        void StartAsync();
        
        JobStatus Status { get; }
        event EventHandler SubJobStarted;
        event EventHandler SubJobCompleted;
        event EventHandler StatusChanged;

        void CheckStatus();
        
        IDictionary<Type, object> Resources { get; }

        ICollection<ISubJob> Dependencies { get; }        
    }
}
