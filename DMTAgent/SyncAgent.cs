using DMTAgent.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DMTAgent
{
    public class SyncAgent : ISyncAgent
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion Fields

        #region Constructors

        public SyncAgent(ISyncManager syncManager,
            ILogger<SyncAgent> logger)
        {
            _logger = logger;
            SyncManager = syncManager;
            SyncManager.JobController.JobCompleted += OnSyncCompleted;
        }

        #endregion Constructors

        #region Events

        public event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        public Timer CurrentTimer { get; set; }
        public AgentStatus Status { get; set; } = AgentStatus.Stopped;
        public ISyncManager SyncManager { get; private set; }
        public TimeSpan UpdateTimeSlack { get; protected set; } = new TimeSpan(0, 15, 0);

        #endregion Properties

        #region Methods

        public void Start()
        {
            OnStart();
        }

        public virtual void Stop()
        {
            OnStop();
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

        protected virtual void ChangeStatus(AgentStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }

        protected virtual void OnStart()
        {
            ScheduleNextUpdate();
            ChangeStatus(AgentStatus.Running);
        }

        protected virtual void OnStop()
        {
            CurrentTimer.Dispose();
            CurrentTimer = null;
            ChangeStatus(AgentStatus.Stopped);
        }

        protected virtual void RaiseStatusChanged() => StatusChanged?.Invoke(this, new EventArgs());

        protected void ScheduleNextUpdate()
        {
            CurrentTimer = new Timer(new TimerCallback(RunUpdate));

            DateTime timeOfNextUpdate = CalculateTimeForNextUpdate(SyncManager.GetUpdateSchedule());
            TimeSpan timeToNextUpdate = (timeOfNextUpdate <= DateTime.Now) ? new TimeSpan(0) : new TimeSpan(timeOfNextUpdate.Ticks - DateTime.Now.Ticks);

            CurrentTimer.Change(timeToNextUpdate, Timeout.InfiniteTimeSpan);

            _logger.LogInformation("Schedulato nuovo task : {0}", new object[] { timeOfNextUpdate });
        }

        private void OnSyncCompleted(object sender, EventArgs e)
        {
            if (Status == AgentStatus.Running)
                ScheduleNextUpdate();
        }

        private void RunUpdate(object e)
        {
            SyncManager.SyncOutdatedElements();
        }

        #endregion Methods
    }
}