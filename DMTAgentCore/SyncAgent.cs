using DMTAgent.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DMTAgentCore
{
    public class SyncAgent
    {
        private readonly ILogger _logger;
        public enum AgentStatus
        {
            Running,
            Stopped,
            Idle
        }

        public SyncAgent(ISyncManager syncManager,
            ILogger logger)
        {
            _logger = logger;
            SyncManager = syncManager;
            SyncManager.JobController.JobCompleted += OnSyncCompleted;
        }


        public event EventHandler ServiceStarted;

        public event EventHandler ServiceStopped;


        public Timer CurrentTimer { get; set; }
        public AgentStatus Status { get; set; } = AgentStatus.Stopped;
        public ISyncManager SyncManager { get; private set; }
        public TimeSpan UpdateTimeSlack { get; protected set; } = new TimeSpan(0, 15, 0);

        public void Start()
        {
            OnStart();
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

        public virtual void Stop()
        {
            OnStop();
        }

        protected virtual void OnStart()
        {
            Status = AgentStatus.Running;
            ScheduleNextUpdate();
            RaiseServiceStarted();
        }

        protected virtual void OnStop()
        {
            Status = AgentStatus.Stopped;
            CurrentTimer.Dispose();
            CurrentTimer = null;
            RaiseServiceStopped();
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

    }
}
