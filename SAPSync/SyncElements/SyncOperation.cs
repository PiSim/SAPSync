using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public abstract class INOperation<TIn> : SyncOperationBase, IINOperation<TIn>
    {
        public Type InputType => typeof(TIn);
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
        protected ICollection<TIn> InputBuffer { get; }
    }

    public abstract class OUTOperation<TOut> : SyncOperationBase, IOUTOperation<TOut>
    {
        public IINOperation<TOut> CurrentOutputTarget { get; protected set; }

        protected virtual void OutputPacket(TOut newPacket)
        {
            if (CurrentOutputTarget != null)
                CurrentOutputTarget.InputPacket(newPacket);
        }

        public virtual void SetOutputTarget(IINOperation<TOut> input)
        {
            CurrentOutputTarget = input;
        }
    }

    public abstract class INOUTOperation<TIn, TOut> : INOperation<TIn>, IINOperation<TIn>, IOUTOperation<TOut>
    {
        public IINOperation<TOut> CurrentOutputTarget { get; protected set; }

        protected virtual void OutputPacket(TOut newPacket)
        {
            if (CurrentOutputTarget != null)
                CurrentOutputTarget.InputPacket(newPacket);
        }

        public virtual void SetOutputTarget(IINOperation<TOut> input)
        {
            CurrentOutputTarget = input;
        }
    }


    public abstract class SyncOperationBase : ISyncOperation
    {
        public abstract string Name { get; }
        public Task CurrentTask { get; protected set; }
        public virtual bool ForceProcessCompletion => true;
        public ISyncElement ParentElement { get; protected set; }
        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;

        public event EventHandler OperationCompleted;

        protected virtual void RaiseOperationCompleted()
        {
            OperationCompleted?.Invoke(this, new EventArgs());
        }

        public virtual void SetParent (ISyncElement syncElement)
        {
            ParentElement = syncElement;
        }

        public virtual void Start()
        {
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

        public virtual async void StartAsync()
        {
            await Task.Run(() => Start());
        }

        public virtual void LoadResources()
        {
            if (ParentElement.CurrentJob == null)
                throw new InvalidOperationException("No open Job");
        }

        public virtual void CheckResourcesLoaded()
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
    }

    public interface IINOperation<TIn>
    {
        void InputPacket(TIn newPacket);
    }

    public interface IOUTOperation<TOut>
    {
        IINOperation<TOut> CurrentOutputTarget { get; }
        void SetOutputTarget(IINOperation<TOut> input);
    }
}
