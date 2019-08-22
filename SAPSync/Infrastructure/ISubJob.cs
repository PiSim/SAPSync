using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface ISubJob
    {
        ISyncElement TargetElement { get;}
        Task CurrentTask { get; }

        void Start();
        void StartAsync();
        
        JobStatus Status { get; }
        event EventHandler OnCompleted;
        event EventHandler OnStarting;
        event EventHandler OnStarted;
        event EventHandler StatusChanged;

        void Complete(bool isSuccesful = true);

        void CheckStatus();
        
        IDictionary<Type, object> Resources { get; }

        ICollection<ISubJob> Dependencies { get; }        
    }
}
