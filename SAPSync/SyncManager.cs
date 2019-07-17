using SAPSync.Functions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncJobs;
using SSMD;
using SyncService;
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

        public DateTime? GetTimeForNextUpdate() => SyncElements.Min(sel => sel.NextScheduledUpdate);

        public void StartSync(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Count() != 0 && !UpdateRunning)
            {
                SyncTask newTask = new SyncTask(syncElements);
                SyncTaskController.RunTask(newTask);
            }
        }
        
        public void SyncOutdatedElements()
        {
            StartSync(SyncElements.Where(sel => sel.NextScheduledUpdate < DateTime.Now).ToList());
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e) => SyncLogger.LogSyncError(e);        

        #endregion Methods
    }
}