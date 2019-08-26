using DataAccessCore;
using SAPSync.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SSMDReader<T> : IRecordReader<T> where T : class
    {
        #region Constructors

        public SSMDReader(Func<Query<T, SSMDContext>> getQueryDelegate = null)
        {
            ChildrenTasks = new List<Task>();

            if (getQueryDelegate != null)
                GetQueryFunc = getQueryDelegate;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        public event EventHandler ReadCompleted;

        public event EventHandler<RecordPacketCompletedEventArgs<T>> RecordPacketCompleted;

        #endregion Events

        #region Properties

        public ICollection<Task> ChildrenTasks { get; }

        protected virtual Func<Query<T, SSMDContext>> GetQueryFunc { get; } = new Func<Query<T, SSMDContext>>(() => new Query<T, SSMDContext>());

        #endregion Properties

        #region Methods

        public void CloseReader()
        {
        }

        public void OpenReader()
        {
        }

        public virtual async void StartReadAsync() => await Task.Run(() => ReadRecords());

        protected virtual Query<T, SSMDContext> GetQuery() => GetQueryFunc();

        protected virtual SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        protected virtual void RaisePacketCompleted(IEnumerable<T> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<T>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());

        protected virtual void ReadRecords()
        {
            IEnumerable<T> results = RunQuery().ToList();
            RaisePacketCompleted(results);
            RaiseReadCompleted();
        }

        protected virtual IQueryable<T> RunQuery() => GetSSMDData().RunQuery(GetQuery());

        protected virtual Task StartChildTask(Action action)
        {
            Task newTask = new Task(action);
            ChildrenTasks.Add(newTask);
            newTask.Start();
            return newTask;
        }

        #endregion Methods
    }

    public class SSMDReader<TQueried, TOut> : IRecordReader<TOut> where TQueried : class
    {
        #region Constructors

        public SSMDReader(Func<IQueryable<TQueried>, IQueryable<TOut>> translatorDelegate,
            Func<Query<TQueried, SSMDContext>> getQueryDelegate = null)
        {
            ChildrenTasks = new List<Task>();
            TranslatorFunc = translatorDelegate;
            GetQueryFunc = getQueryDelegate;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        public event EventHandler ReadCompleted;

        public event EventHandler<RecordPacketCompletedEventArgs<TOut>> RecordPacketCompleted;

        #endregion Events

        #region Properties

        public ICollection<Task> ChildrenTasks { get; }

        protected virtual Func<Query<TQueried, SSMDContext>> GetQueryFunc { get; } = new Func<Query<TQueried, SSMDContext>>(() => new Query<TQueried, SSMDContext>());

        protected virtual Func<IQueryable<TQueried>, IQueryable<TOut>> TranslatorFunc { get; }

        #endregion Properties

        #region Methods

        public void CloseReader()
        {
        }

        public void OpenReader()
        {
        }

        public virtual IEnumerable<TOut> ReadRecords() => RunQuery().ToList();

        public virtual async void StartReadAsync() => await Task.Run(() => ReadRecords());

        protected virtual SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        protected virtual void RaisePacketCompleted(IEnumerable<TOut> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<TOut>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());

        protected virtual IQueryable<TOut> RunQuery() => TranslatorFunc(GetSSMDData().RunQuery(GetQueryFunc()));

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