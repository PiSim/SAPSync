using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Infrastructure;
using DataAccessCore;
using DataAccessCore.Commands;

namespace SAPSync.SyncElements
{

    public abstract class SyncElementBase : ISyncElement
    {
        public SyncElementBase(string name = "", SyncElementConfiguration configuration = null)
        {
            Name = name;
            Configuration = configuration ?? new SyncElementConfiguration();
            ReadElementData();
            Dependencies = new List<ISyncElement>();
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

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        public event EventHandler ElementStarting;
        public event EventHandler ElementCompleted;

        public virtual string Name { get; }
        public ISubJob CurrentJob { get; protected set; }

        public SyncElementConfiguration Configuration { get; private set; }
        public SyncElementData ElementData { get; protected set; }

        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        public ICollection<ISyncElement> Dependencies { get; }

        protected virtual DateTime GetNextScheduledUpdate() => ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);

        protected virtual void OpenJob(ISubJob newJob)
        {

            if (CurrentJob != null)
                throw new InvalidOperationException("Another Job is already open");
            CurrentJob = newJob;
        }

        protected virtual void CloseJob()
        {
            CurrentJob.Complete();
            CurrentJob = null;
        }

        public virtual void Execute(ISubJob newJob)
        {
            OpenJob(newJob);
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
    }
}
