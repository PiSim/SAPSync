using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncManager : ISyncManager
    {
        #region Constructors

        public SyncManager()
        {
            JobController = new JobController();
            JobController.SyncErrorRaised += OnSyncErrorRaised;
            JobController.JobStarting += OnTaskStarting;
            JobController.JobCompleted += OnTaskCompleted;
            SyncElements = (new SyncElementFactory()).BuildSyncElements();
        }

        #endregion Constructors

        #region Properties

        public IJobController JobController { get; }
        public ICollection<ISyncElement> SyncElements { get; set; }

        public bool UpdateRunning => JobController.ActiveJobs.Count != 0;

        #endregion Properties

        #region Methods

        public IEnumerable<DateTime?> GetUpdateSchedule() => SyncElements.Select(sel => sel.NextScheduledUpdate);

        public void StartSync(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Count() != 0 && !UpdateRunning)
            {
                JobController.StartJob(syncElements);
            }
        }

        public void SyncOutdatedElements()
        {
            StartSync(SyncElements.Where(sel => sel.NextScheduledUpdate < DateTime.Now).ToList());
        }

        protected virtual void OnElementCompleted(object sender, EventArgs e)
        {
            if (sender is ISyncElement)
                SyncLogger.LogElementCompleted(sender as ISyncElement);
        }

        protected virtual void OnElementStarting(object sender, EventArgs e)
        {
            if (sender is ISyncElement)
                SyncLogger.LogElementStarting(sender as ISyncElement);
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e) => SyncLogger.LogSyncError(e);

        protected virtual void OnTaskCompleted(object sender, EventArgs e)
        {
            if (sender is IJob)
                SyncLogger.LogTaskCompleted(sender as IJob);
        }

        protected virtual void OnTaskStarting(object sender, EventArgs e)
        {
            if (sender is IJob)
                SyncLogger.LogTaskStarting(sender as IJob);
        }

        #endregion Methods
    }
}