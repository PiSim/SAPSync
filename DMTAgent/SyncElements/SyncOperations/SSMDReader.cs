using DataAccessCore;
using DMTAgent.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMTAgent
{
    public class SSMDReader<T> : IRecordReader<T> where T : class
    {
        #region Fields

        private readonly IDataService<SSMDContext> _dataService;

        #endregion Fields

        #region Constructors

        public SSMDReader(IDataService<SSMDContext> dataService,
            Func<Query<T, SSMDContext>> getQueryDelegate = null)
        {
            _dataService = dataService;
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

        protected virtual IQueryable<T> RunQuery() => _dataService.RunQuery(GetQuery());

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
        #region Fields

        private readonly IDataService<SSMDContext> _dataService;

        #endregion Fields

        #region Constructors

        public SSMDReader(IDataService<SSMDContext> dataService,
            Func<IQueryable<TQueried>, IQueryable<TOut>> translatorDelegate,
            Func<Query<TQueried, SSMDContext>> getQueryDelegate = null)
        {
            _dataService = dataService;
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


        protected virtual void ReadRecords()
        {
            IEnumerable<TOut> results = RunQuery().ToList();
            RaisePacketCompleted(results);
            RaiseReadCompleted();
        }

        public virtual async void StartReadAsync() => await Task.Run(() => ReadRecords());

        protected virtual void RaisePacketCompleted(IEnumerable<TOut> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<TOut>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());

        protected virtual IQueryable<TOut> RunQuery() => TranslatorFunc(_dataService.RunQuery(GetQueryFunc()));

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