using SAPSync.Functions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncJobs;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncManager : ISyncManager
    {

        #region Constructors

        public SyncManager()
        {
            SyncTaskController = new SyncTaskController();
            SyncTaskController.SyncErrorRaised += OnSyncErrorRaised;
            SyncTaskController.SyncTaskStarting += OnTaskStarting;
            SyncTaskController.SyncTaskCompleted += OnTaskCompleted;
            SyncElements = (new SyncElementFactory()).BuildSyncElements();

            foreach (ISyncElement element in SyncElements)
                element.SetTaskController(SyncTaskController);
        }

        #endregion Constructors


        #region Properties

        public ISyncTaskController SyncTaskController { get; }
        public ICollection<ISyncElement> SyncElements { get; set; }
        
        public bool UpdateRunning => SyncTaskController.ActiveTasks.Count != 0;

        #endregion Properties

        #region Methods

        public IEnumerable<DateTime?> GetUpdateSchedule() => SyncElements.Select(sel => sel.NextScheduledUpdate);

        public void StartSync(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Count() != 0 && !UpdateRunning)
            {
                SyncTask newTask = new SyncTask(syncElements);
                SubscribeToTask(newTask);
                SyncTaskController.RunTask(newTask);
            }
        }

        protected virtual void SubscribeToTask(ISyncTask task)
        {
            task.ElementStarting += OnElementStarting;
            task.ElementCompleted += OnElementCompleted;
        }
        
        protected virtual void OnElementStarting(object sender, EventArgs e)
        {
            if (sender is ISyncElement)
                SyncLogger.LogElementStarting(sender as ISyncElement);
        }

        protected virtual void OnElementCompleted(object sender, EventArgs e)
        {

            if (sender is ISyncElement)
                SyncLogger.LogElementCompleted(sender as ISyncElement);
        }

        protected virtual void OnTaskStarting(object sender, EventArgs e)
        {
            if (sender is ISyncTask)
                SyncLogger.LogTaskStarting(sender as ISyncTask);

        }
        protected virtual void OnTaskCompleted(object sender, EventArgs e)
        {

            if (sender is ISyncTask)
                SyncLogger.LogTaskCompleted(sender as ISyncTask);
        }

        public void SyncOutdatedElements()
        {
            StartSync(SyncElements.Where(sel => sel.NextScheduledUpdate < DateTime.Now).ToList());
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e) => SyncLogger.LogSyncError(e);        

        #endregion Methods
    }
}