using SyncService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public enum SyncJobStatus
    {
        Created,
        Running,
        Completed,
        Failed,
        Interrupted
    }

    public interface ISyncJob : ISyncBase, IDisposable
    {
        SyncJobStatus Status { get; }
        void Run();
    }
}
