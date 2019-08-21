using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IJob : ISyncBase
    {
        Task CurrentTask { get; }
        
        void Start();

        event EventHandler JobCompleted;
        List<string> SyncLog { get; set; }
        List<Task> TaskList { get; }
        event EventHandler JobStarting;
        event EventHandler JobStarted;
        
        ICollection<ISyncElement> SyncElementsStack { get; }

        ICollection<ISubJob> SubJobs { get; }
    }
}
