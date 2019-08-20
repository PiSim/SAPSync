using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface ISyncTask : ISyncBase
    {
        void Start();
        event EventHandler SyncTaskCompleted;
        List<string> SyncLog { get; set; }
        List<Task> TaskList { get; }
        event EventHandler SyncTaskStarting;
        event EventHandler SyncTaskStarted;
        
        ICollection<ISyncElement> PendingSyncElements { get; }
        ICollection<Task> ActiveReadTasks { get; }
    }
}
