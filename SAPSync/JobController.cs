﻿using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync
{
    public class JobController : IJobController
    {

        #region Events

        public event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
        public event EventHandler NewJobStarted;

        public event EventHandler JobCompleted;
        public event EventHandler JobStarting;

        #endregion Events

        public JobController()
        {
            ActiveJobs = new HashSet<IJob>();
            CompletedJobs = new HashSet<IJob>();
        }

        public ICollection<IJob> ActiveJobs { get; }
        public ICollection<IJob> CompletedJobs { get; }

        public Task GetAwaiterForActiveOperations() => Task.WhenAll(
            ActiveJobs.SelectMany(sts => sts.SubJobs)
                .SelectMany(sjb => sjb.UnitsOfWork)
                .Select(uow => uow.CurrentTask)
                .ToList());
        

        public void StartJob(IJob task)
        {
            task.JobCompleted += OnJobCompleted;
            task.SyncErrorRaised += OnSyncErrorRaised;
            task.JobStarting += OnSyncTaskStarting;
            task.JobStarted += OnSyncTaskStarted;
            ActiveJobs.Add(task);
            task.Start();
            
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            SyncErrorRaised?.Invoke(sender, e);
        }

        protected virtual void OnJobCompleted(object sender, EventArgs e)
        {
            Job completedTask = sender as Job;
            if (ActiveJobs.Contains(completedTask))
                ActiveJobs.Remove(completedTask);
            CompletedJobs.Add(completedTask);
            JobCompleted?.Invoke(sender, e);
        }

        protected virtual void OnSyncTaskStarting(object sender, EventArgs e) => JobStarting?.Invoke(sender, e);
        

        protected virtual void OnSyncTaskStarted(object sender, EventArgs e) => NewJobStarted?.Invoke(sender, e);
    }
}