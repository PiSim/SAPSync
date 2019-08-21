using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public enum SyncOperationStatus
    {
        Created,
        Running,
        Completed,
        Failed,
        Interrupted
    }

    public interface ISyncOperation :  IDisposable
    {        
        Task CurrentTask { get; }
        
        void Open();            
               
        void Close();
    }
}
