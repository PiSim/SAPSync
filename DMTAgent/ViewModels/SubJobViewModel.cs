using DMTAgent.Infrastructure;
using System;

namespace DMTAgent.ViewModels
{
    public class SubJobViewModel : BindableBase
    {
        #region Constructors

        public SubJobViewModel(ISubJob subJob)
        {
            SubJob = subJob;
            SubJob.StatusChanged += OnStatusChanged;
        }

        #endregion Constructors

        #region Properties

        public DateTime EndTime => SubJob.EndTime;

        public JobStatus JobStatus => SubJob.Status;

        public string Name => SubJob.TargetElement.Name;

        public DateTime StartTime => SubJob.StartTime;

        public ISubJob SubJob { get; }

        #endregion Properties

        #region Methods

        protected virtual void OnStatusChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("JobStatus");
            RaisePropertyChanged("StartTime");
            RaisePropertyChanged("EndTime");
        }

        #endregion Methods
    }
}