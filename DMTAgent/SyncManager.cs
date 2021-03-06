﻿using DMTAgent.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMTAgent
{
    public class SyncManager : ISyncManager
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion Fields

        #region Constructors

        public SyncManager(ILogger<SyncManager> logger,
            ISyncElementFactory elementFactory)
        {
            _logger = logger;
            JobController = new JobController();
            JobController.SyncErrorRaised += OnSyncErrorRaised;
            JobController.JobStarting += OnTaskStarting;
            JobController.JobCompleted += OnTaskCompleted;
            SyncElements = elementFactory.BuildSyncElements();
        }

        #endregion Constructors

        #region Properties

        public IJobController JobController { get; }
        public IEnumerable<ISyncElement> SyncElements { get; }

        #endregion Properties

        #region Methods

        public IEnumerable<DateTime?> GetUpdateSchedule() => SyncElements.Select(sel => sel.NextScheduledUpdate);

        public void StartSync(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Count() != 0)
            {
                JobController.StartJob(syncElements);
            }
        }

        public void SyncOutdatedElements()
        {
            StartSync(SyncElements.Where(sel => sel.NextScheduledUpdate < DateTime.Now).ToList());
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
            => _logger.LogError(e.Exception, "Errore di sincronizzazione", new object[] { });

        protected virtual void OnTaskCompleted(object sender, EventArgs e)
        {
            if (sender is IJob)
                _logger.LogInformation("Task completato", new object[] { (sender as IJob) });
        }

        protected virtual void OnTaskStarting(object sender, EventArgs e)
        {
            if (sender is IJob)
                _logger.LogInformation("Avvio nuovo task", new object[] { (sender as IJob) });
        }

        #endregion Methods
    }
}