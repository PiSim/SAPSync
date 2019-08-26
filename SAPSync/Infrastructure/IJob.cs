using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public interface IJob
    {
        #region Events

        event EventHandler OnCompleted;

        event EventHandler OnStarted;

        event EventHandler OnStarting;

        event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        Task CurrentTask { get; }

        JobStatus Status { get; }

        ICollection<ISubJob> SubJobs { get; }

        ICollection<ISyncElement> SyncElementsStack { get; }

        #endregion Properties

        #region Methods

        void Start();

        void StartAsync();

        #endregion Methods
    }
}