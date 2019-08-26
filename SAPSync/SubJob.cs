using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SubJob : JobBase, ISubJob
    {
        #region Constructors

        public SubJob(ISyncElement targetElement)
        {
            Status = JobStatus.OnQueue;
            TargetElement = targetElement;
            Dependencies = new List<ISubJob>();
        }

        #endregion Constructors

        #region Properties

        public ICollection<ISubJob> Dependencies { get; }
        public IDictionary<Type, object> Resources { get; }
        public ISyncElement TargetElement { get; }

        #endregion Properties

        #region Methods

        public void CheckStatus()
        {
            if (Status == JobStatus.OnQueue && Dependencies.All(dep => dep.Status == JobStatus.Completed))
                ChangeStatus(JobStatus.Ready);
        }

        public void Complete(bool isSuccesful = true)
        {
            if (isSuccesful)
                ChangeStatus(JobStatus.Completed);
            else
                ChangeStatus(JobStatus.Failed);

            RaiseOnCompleted();
        }

        public override void Start()
        {
            if (Status != JobStatus.Ready)
                throw new InvalidOperationException("Job is not in Ready state");

            ChangeStatus(JobStatus.Running);
            TargetElement.Execute(this);
        }

        #endregion Methods
    }
}