using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface ISubJob
    {
        #region Events

        event EventHandler OnCompleted;

        event EventHandler OnStarted;

        event EventHandler OnStarting;

        event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        Task CurrentTask { get; }
        ICollection<ISubJob> Dependencies { get; }
        IDictionary<Type, object> Resources { get; }
        JobStatus Status { get; }
        ISyncElement TargetElement { get; }

        #endregion Properties

        #region Methods

        void CheckStatus();

        void Complete(bool isSuccesful = true);

        void Start();

        void StartAsync();

        #endregion Methods
    }
}