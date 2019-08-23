using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface ISyncOperation
    {        
        string Name { get; }
        ICollection<Task> ChildrenTasks { get; }
        ISubJob CurrentJob { get; }
        void SetParent(ISyncElement syncElement);
        void Start(ISubJob newJob);
        void StartAsync(ISubJob newJob);
        event EventHandler OperationCompleted;
        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
    }
}
