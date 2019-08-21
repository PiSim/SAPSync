using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public abstract class JobBase
    {
        public abstract void Start();

        public virtual async void StartAsync() => await Task.Run(() => Start());

        public event EventHandler OnCompleted;
        public event EventHandler OnStarting;
        public event EventHandler OnStarted;
        public event EventHandler StatusChanged;

        public Task CurrentTask { get; protected set; }

        protected virtual void RaiseOnStarting() => OnStarting?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnCompleted() => OnCompleted?.Invoke(this, new EventArgs());
        protected virtual void RaiseStatusChanged() => StatusChanged?.Invoke(this, new EventArgs());
        protected virtual void RaiseOnStarted() => OnStarted?.Invoke(this, new EventArgs());

        protected virtual void RaiseCompleted() => OnCompleted?.Invoke(this, new EventArgs());
        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        public JobStatus Status { get; protected set; }
        
        protected virtual void ChangeStatus(JobStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }
    }
}
