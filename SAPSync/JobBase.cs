using SAPSync.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SAPSync
{
    public abstract class JobBase
    {
        #region Events

        public event EventHandler OnCompleted;

        public event EventHandler OnStarted;

        public event EventHandler OnStarting;

        public event EventHandler StatusChanged;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        public Task CurrentTask { get; protected set; }

        public JobStatus Status { get; protected set; }

        #endregion Properties

        #region Methods

        public abstract void Start();

        public virtual async void StartAsync() => await Task.Run(() => Start());

        protected virtual void ChangeStatus(JobStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }

        protected virtual void RaiseCompleted() => OnCompleted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnCompleted() => OnCompleted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnStarted() => OnStarted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnStarting() => OnStarting?.Invoke(this, new EventArgs());

        protected virtual void RaiseStatusChanged() => StatusChanged?.Invoke(this, new EventArgs());

        #endregion Methods
    }
}