using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncService
{
    public interface ISyncTask
    {
        void Start();
        event EventHandler SyncTaskCompleted;
        List<string> SyncLog { get; set; }
        List<Task> TaskList { get; }
        event EventHandler SyncTaskStarting;
        event EventHandler SyncTaskStarted;
        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
        
        ICollection<ISyncElement> PendingSyncElements { get; }
        ICollection<Task> ActiveReadTasks { get; }
    }
}
