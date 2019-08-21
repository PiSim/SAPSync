using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsFinal { get; }

        Task CurrentTask { get; }

        ISyncOperation CurrentOperation { get; }
        
        ISubJob ParentSubJob { get; }

        bool MustAwaitCompletion { get; }
    }
}
