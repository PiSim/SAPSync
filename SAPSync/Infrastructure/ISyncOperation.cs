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
        Task CurrentTask { get; }
        void SetParent(ISyncElement syncElement);
        void Start();
        void StartAsync();
        event EventHandler OperationCompleted;
        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
    }
}
