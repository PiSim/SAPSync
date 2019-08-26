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

        public DateTime EndTime { get; protected set; }
        public DateTime StartTime { get; protected set; }
        public JobStatus Status { get; protected set; }

        #endregion Properties

        #region Methods

        public virtual void Start()
        {
            StartTime = DateTime.Now;
        }

        public virtual async void StartAsync() => await Task.Run(() => Start());

        protected virtual void ChangeStatus(JobStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }

        protected virtual void Complete(bool isSuccesful = true)
        {
            if (isSuccesful)
                ChangeStatus(JobStatus.Completed);
            else
                ChangeStatus(JobStatus.Failed);
            RaiseCompleted();
            EndTime = DateTime.Now;
        }

        protected virtual void RaiseCompleted() => OnCompleted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnCompleted() => OnCompleted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnStarted() => OnStarted?.Invoke(this, new EventArgs());

        protected virtual void RaiseOnStarting() => OnStarting?.Invoke(this, new EventArgs());

        protected virtual void RaiseStatusChanged() => StatusChanged?.Invoke(this, new EventArgs());

        protected virtual void RaiseSyncError(
                                                            Exception e = null,
            string errorMessage = null,
            SyncErrorEventArgs.ErrorSeverity errorSeverity = SyncErrorEventArgs.ErrorSeverity.Minor)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                NameOfElement = ToString(),
                Severity = errorSeverity,
                ErrorMessage = errorMessage,
                TimeStamp = DateTime.Now,
                TypeOfElement = GetType()
            };

            SyncErrorRaised?.Invoke(this, args);
        }

        #endregion Methods
    }
}