﻿using SAPSync.RFCFunctions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncOperations;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync
{
    public class SyncManager : ISyncManager
    {

        #region Constructors

        public SyncManager()
        {
            SyncTaskController = new JobController();
            SyncTaskController.SyncErrorRaised += OnSyncErrorRaised;
            SyncTaskController.JobStarting += OnTaskStarting;
            SyncTaskController.JobCompleted += OnTaskCompleted;
            SyncElements = (new SyncElementFactory()).BuildSyncElements();

            foreach (ISyncElement element in SyncElements)
                element.SetJobController(SyncTaskController);
        }

        #endregion Constructors


        #region Properties

        public IJobController SyncTaskController { get; }
        public ICollection<ISyncElement> SyncElements { get; set; }
        
        public bool UpdateRunning => SyncTaskController.ActiveJobs.Count != 0;

        #endregion Properties

        #region Methods

        public IEnumerable<DateTime?> GetUpdateSchedule() => SyncElements.Select(sel => sel.NextScheduledUpdate);

        public void StartSync(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Count() != 0 && !UpdateRunning)
            {
                Job newTask = new Job(syncElements);
                SubscribeToTask(newTask);
                SyncTaskController.StartJob(newTask);
            }
        }

        protected virtual void SubscribeToTask(IJob task)
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
            if (sender is IJob)
                SyncLogger.LogTaskStarting(sender as IJob);

        }
        protected virtual void OnTaskCompleted(object sender, EventArgs e)
        {

            if (sender is IJob)
                SyncLogger.LogTaskCompleted(sender as IJob);
        }

        public void SyncOutdatedElements()
        {
            StartSync(SyncElements.Where(sel => sel.NextScheduledUpdate < DateTime.Now).ToList());
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e) => SyncLogger.LogSyncError(e);        

        #endregion Methods
    }
}