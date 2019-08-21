using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface IJob
    {        
        Task CurrentTask { get; }
        
        void Start();
        void StartAsync();

        JobStatus Status { get; }
        event EventHandler JobCompleted;
        event EventHandler JobStarting;
        event EventHandler JobStarted;
        event EventHandler StatusChanged;

        ICollection<ISyncElement> SyncElementsStack { get; }
        ICollection<ISubJob> SubJobs { get; }
    }
}
