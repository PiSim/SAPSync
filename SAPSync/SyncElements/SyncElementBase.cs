using DataAccessCore;
using DataAccessCore.Commands;
using SAPSync.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public abstract class SyncElementBase : ISyncElement
    {
        #region Constructors

        public SyncElementBase(string name = "", SyncElementConfiguration configuration = null)
        {
            Name = name;
            Configuration = configuration ?? new SyncElementConfiguration();
            ReadElementData();
            Dependencies = new List<ISyncElement>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler ElementCompleted;

        public event EventHandler ElementStarting;

        #endregion Events

        #region Properties

        public SyncElementConfiguration Configuration { get; private set; }

        public ISubJob CurrentJob { get; protected set; }

        public ICollection<ISyncElement> Dependencies { get; }

        public SyncElementData ElementData { get; protected set; }

        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public virtual string Name { get; }

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods

        public virtual void Execute(ISubJob newJob)
        {
            OpenJob(newJob);
        }

        protected virtual void CloseJob()
        {
            CurrentJob.CloseJob();
            CurrentJob = null;
        }

        protected virtual void FinalizeSync()
        {
            CloseJob();
            ElementData.LastUpdate = DateTime.Now;
            try
            {
                SaveElementData();
            }
            catch (Exception e)
            {
                throw new Exception("Impossibile salvare ElementData: " + e.Message, e);
            }
        }

        protected virtual DateTime GetNextScheduledUpdate() => ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);

        protected virtual void OpenJob(ISubJob newJob)
        {
            if (CurrentJob != null)
                throw new InvalidOperationException("Another Job is already open");
            CurrentJob = newJob;
        }

        protected virtual void ReadElementData()
        {
            ElementData = SSMDData.RunQuery(new Query<SyncElementData, SSMDContext>()).FirstOrDefault(sed => sed.ElementType == this.Name);
            if (ElementData == null)
                ElementData = new SyncElementData()
                {
                    LastUpdate = new DateTime(0),
                    ElementType = Name
                };
        }

        protected virtual void SaveElementData()
        {
            SSMDData.Execute(new UpdateEntityCommand<SSMDContext>(ElementData));
        }

        #endregion Methods
    }
}