using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Interfaces
{
    public enum SyncOperationStatus
    {
        Created,
        Running,
        Completed,
        Failed,
        Interrupted
    }

    public interface ISyncOperation : ISyncBase, IDisposable
    {
        SyncOperationStatus Status { get; }

        // Deprecated interface element. Refactored in order to ease batching & async implementation
        // void Run();


        void Open();

        
               
        void Close();
    }
}
