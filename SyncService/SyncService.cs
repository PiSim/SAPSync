using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace SyncService
{
    public class SyncService : ServiceBase
    {
        #region Constructors

        public TimeSpan UpdateTimeSlack { get; protected set; } = new TimeSpan(0, 15, 0);

        public SyncService(ISyncManager syncManager)
        {
            SyncManager = syncManager;
            SyncManager.SyncTaskController.SyncTaskCompleted += OnSyncCompleted;
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
            CurrentTimer = null;
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



            DateTime timeOfNextUpdate = CalculateTimeForNextUpdate(SyncManager.GetUpdateSchedule());
            TimeSpan timeToNextUpdate = (timeOfNextUpdate <= DateTime.Now) ? new TimeSpan(0) : new TimeSpan(timeOfNextUpdate.Ticks - DateTime.Now.Ticks);

            CurrentTimer.Change(timeToNextUpdate, Timeout.InfiniteTimeSpan);

            SyncLogger.LogTaskScheduled(timeOfNextUpdate);
        }


        protected virtual DateTime CalculateTimeForNextUpdate(IEnumerable<DateTime?> updateSchedule)
        {
            if (updateSchedule.Count() == 0)
                return DateTime.Now;

            else
            {
                DateTime? baseTime = updateSchedule.Min();
                DateTime? output = updateSchedule.Where(tmg => tmg <= (baseTime + UpdateTimeSlack)).Max();
                return output ?? DateTime.Now;
            }

        }

        private void OnSyncCompleted(object sender, EventArgs e)
        {
            if (Status == ServiceControllerStatus.Running)
                ScheduleNextUpdate();
        }

        private void RunUpdate(object e)
        {
            SyncManager.SyncOutdatedElements();
        }

        #endregion Methods
    }
}