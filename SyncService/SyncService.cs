using System;
using System.ServiceProcess;
using System.Threading;

namespace SyncService
{
    public class SyncService : ServiceBase
    {
        #region Constructors

        public SyncService(ISyncManager syncManager)
        {
            SyncManager = syncManager;
            SyncManager.SyncTaskCompleted += OnSyncCompleted;
        }

        #endregion Constructors

        #region Events

        public event EventHandler ServiceStarted;

        public event EventHandler ServiceStopped;

        #endregion Events

        #region Properties

        public Timer CurrentTimer { get; set; }

        public ServiceControllerStatus Status { get; set; } = ServiceControllerStatus.Stopped;
        public ISyncManager SyncManager { get; private set; }

        #endregion Properties

        #region Methods

        public void Start()
        {
            OnStart(new string[] { });
        }

        protected override void OnStart(string[] args)
        {
            Status = ServiceControllerStatus.Running;
            base.OnStart(args);
            ScheduleNextUpdate();
            RaiseServiceStarted();
        }

        protected override void OnStop()
        {
            Status = ServiceControllerStatus.Stopped;
            CurrentTimer.Dispose();
            RaiseServiceStopped();
            base.OnStop();
        }

        protected virtual void RaiseServiceStarted()
        {
            ServiceStarted?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseServiceStopped()
        {
            ServiceStopped?.Invoke(this, new EventArgs());
        }

        protected void ScheduleNextUpdate()
        {
            CurrentTimer = new Timer(new TimerCallback(RunUpdate));

            DateTime timeOfNextUpdate = SyncManager.GetTimeForNextUpdate() ?? new DateTime(0);
            TimeSpan timeToNextUpdate = (timeOfNextUpdate <= DateTime.Now) ? new TimeSpan(0) : new TimeSpan(timeOfNextUpdate.Ticks - DateTime.Now.Ticks);

            CurrentTimer.Change(timeToNextUpdate, Timeout.InfiniteTimeSpan);
        }

        private void OnSyncCompleted(object sender, EventArgs e)
        {
            ScheduleNextUpdate();
        }

        private void RunUpdate(object e)
        {
            SyncManager.StartSync();
        }

        #endregion Methods
    }
}