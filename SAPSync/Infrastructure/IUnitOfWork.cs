using System;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        ISyncOperation CurrentOperation { get; }
        Task CurrentTask { get; }
        bool IsFinal { get; }
        bool MustAwaitCompletion { get; }
        ISubJob ParentSubJob { get; }

        #endregion Properties
    }
}