using DMTAgent.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMTAgentCore.SyncElements
{
    public interface IINOperation<TIn>
    {
        #region Methods

        void InputPacket(TIn newPacket);

        #endregion Methods
    }

    public interface IOUTOperation<TOut>
    {
        #region Properties

        IINOperation<TOut> CurrentOutputTarget { get; }

        #endregion Properties

        #region Methods

        void SetOutputTarget(IINOperation<TOut> input);

        #endregion Methods
    }

    public abstract class INOperation<TIn> : SyncOperationBase, IINOperation<TIn>
    {
        #region Properties

        public Type InputType => typeof(TIn);
        protected ICollection<TIn> InputBuffer { get; }

        #endregion Properties

        #region Methods

        public virtual void InputPacket(TIn newPacket)
        {
            InputBuffer.Add(newPacket);
        }

        public TIn Read()
        {
            TIn nextElement = InputBuffer.FirstOrDefault();
            if (nextElement != null)
                InputBuffer.Remove(nextElement);
            return nextElement;
        }

        #endregion Methods
    }

    public abstract class INOUTOperation<TIn, TOut> : INOperation<TIn>, IINOperation<TIn>, IOUTOperation<TOut>
    {
        #region Properties

        public IINOperation<TOut> CurrentOutputTarget { get; protected set; }

        #endregion Properties

        #region Methods

        public virtual void SetOutputTarget(IINOperation<TOut> input)
        {
            CurrentOutputTarget = input;
        }

        protected virtual void OutputPacket(TOut newPacket)
        {
            if (CurrentOutputTarget != null)
                CurrentOutputTarget.InputPacket(newPacket);
        }

        #endregion Methods
    }

    public abstract class OUTOperation<TOut> : SyncOperationBase, IOUTOperation<TOut>
    {
        #region Properties

        public IINOperation<TOut> CurrentOutputTarget { get; protected set; }

        #endregion Properties

        #region Methods

        public virtual void SetOutputTarget(IINOperation<TOut> input)
        {
            CurrentOutputTarget = input;
        }

        protected virtual void OutputPacket(TOut newPacket)
        {
            if (CurrentOutputTarget != null)
                CurrentOutputTarget.InputPacket(newPacket);
        }

        #endregion Methods
    }

    public abstract class SyncOperationBase : ISyncOperation
    {
        #region Constructors

        public SyncOperationBase()
        {
            ChildrenTasks = new List<Task>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler OperationCompleted;

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        #endregion Events

        #region Properties

        public ICollection<Task> ChildrenTasks { get; }
        public ISubJob CurrentJob { get; protected set; }
        public Task CurrentTask { get; protected set; }
        public virtual bool ForceProcessCompletion => true;
        public abstract string Name { get; }
        public ISyncElement ParentElement { get; protected set; }

        #endregion Properties

        #region Methods

        public virtual void CheckResourcesLoaded()
        {
        }

        public virtual void LoadResources()
        {
            if (CurrentJob == null)
                throw new InvalidOperationException("No open Job");
        }

        public virtual void SetParent(ISyncElement syncElement)
        {
            ParentElement = syncElement;
        }

        public virtual void Start(ISubJob newJob)
        {
            OpenJob(newJob);
        }

        public virtual async void StartAsync(ISubJob newJob)
        {
            await Task.Run(() => Start(newJob));
        }

        protected virtual void Clear()
        {
            ChildrenTasks.Clear();
        }

        protected virtual void CloseJob()
        {
            CurrentJob = null;
            RaiseOperationCompleted();
            Clear();
        }

        protected virtual void OpenJob(ISubJob newJob)
        {
            CurrentJob = newJob;

            try
            {
                LoadResources();
                CheckResourcesLoaded();
            }
            catch
            {
                throw new Exception("Failed Loading Resources");
            }
        }

        protected virtual void RaiseOperationCompleted()
        {
            OperationCompleted?.Invoke(this, new EventArgs());
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

        protected virtual Task StartChildTask(Action action)
        {
            Task newTask = new Task(action);
            ChildrenTasks.Add(newTask);
            newTask.Start();
            return newTask;
        }

        #endregion Methods
    }
}