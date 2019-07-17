using SSMD;
using SyncService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{

    public abstract class SyncElementBase : ISyncBase, IDisposable
    {
        public abstract string Name { get; }

        protected virtual void Initialize()
        {

        }
        protected virtual void EnsureInitialized()
        {

        }

        public void Dispose()
        {
            Clear();
        }

        protected virtual void Clear()
        {
        }

        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler<TaskEventArgs> ExternalTaskCompleted;
        public event EventHandler<TaskEventArgs> ExternalTaskStarting;
        public event EventHandler StatusChanged;
        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        protected virtual void RaiseExternalTaskStarting(Task t)
        {
            ExternalTaskStarting?.Invoke(this, new TaskEventArgs(t));
        }
        protected virtual void RaiseExternalTaskCompleted(Task t)
        {
            ExternalTaskCompleted?.Invoke(this, new TaskEventArgs(t));
        }

        protected virtual SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());
               
        public virtual void Run()
        {
            try
            {
                OnStarting();
                Initialize();
                EnsureInitialized();
                Execute();
                OnCompleting();
            }
            catch (Exception e)
            {
                OnFailure();
                RaiseSyncError(e: e,
                    errorMessage: "Errore: " + e.Message + "\t\tInnerException :" + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                throw new Exception(": " + e.Message, e);
            }
        }

        protected virtual void OnFailure()
        {

        }

        protected virtual void OnStarting()
        {

        }

        protected virtual void OnCompleting()
        {

        }

        protected virtual void Execute()
        {
        }

        protected virtual void RaiseSyncError(
            Exception e = null,
            string errorMessage = null,
            SyncErrorEventArgs.ErrorSeverity errorSeverity = SyncErrorEventArgs.ErrorSeverity.Minor)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                NameOfElement = Name,
                Severity = errorSeverity,
                ErrorMessage = errorMessage,
                TimeStamp = DateTime.Now,
                TypeOfElement = GetType()
            };

            SyncErrorRaised?.Invoke(this, args);
        }

        protected virtual void RaiseProgressChanged(int newProgress)
        {
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(newProgress, null);

            ProgressChanged?.Invoke(this, e);
        }
        protected void RaiseStatusChanged()
        {
            EventArgs e = new EventArgs();
            StatusChanged?.Invoke(this, e);
        }

        protected virtual void OnSubscribedExternalTaskCompleted(object sender, TaskEventArgs e)
        {
            ExternalTaskCompleted?.Invoke(sender, e);
        }

        protected virtual void OnSubscribedExternalTaskStarting(object sender, TaskEventArgs e)
        {
            ExternalTaskStarting?.Invoke(sender, e);
        }

        protected virtual void OnSubscribedProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(sender, e);
        }

        protected virtual void OnSubscribedStatusChanged(object sender, EventArgs e)
        {
            StatusChanged?.Invoke(sender, e);
        }

        protected virtual void OnSubscribedSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            SyncErrorRaised?.Invoke(sender, e);
        }

        protected virtual void SubscribeToElement(ISyncBase syncBase)
        {
            syncBase.ExternalTaskCompleted += OnSubscribedExternalTaskCompleted;
            syncBase.ExternalTaskStarting += OnSubscribedExternalTaskStarting;
            syncBase.ProgressChanged += OnSubscribedProgressChanged;
            syncBase.StatusChanged += OnSubscribedStatusChanged;
            syncBase.SyncErrorRaised += OnSubscribedSyncErrorRaised;
        }
    }
}
