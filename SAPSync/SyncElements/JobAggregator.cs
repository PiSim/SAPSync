using DataAccessCore;
using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements
{
    public class JobAggregator : ISyncElement
    {
        #region Constructors

        public JobAggregator(string name = "", SyncElementConfiguration configuration = null)
        {
            Name = name;
            Configuration = configuration ?? new SyncElementConfiguration();
            ReadElementData();
        }
        
        public event EventHandler SyncAborted;

        public event EventHandler SyncCompleted;

        public event EventHandler<SyncErrorEventArgs> SyncFailed;

        #endregion Events

        #region Properties

        public SyncElementConfiguration Configuration { get; private set; }
        public SyncElementData ElementData { get; protected set; }

        public bool IsUpForScheduledUpdate => NextScheduledUpdate <= DateTime.Now;

        public DateTime? LastUpdate => ElementData?.LastUpdate;

        public DateTime? NextScheduledUpdate => GetNextScheduledUpdate();

        public IList<ISyncElement> RequiredElements { get; } = new List<ISyncElement>();

        protected SSMDData SSMDData => new SSMDData(new SSMDContextFactory());

        #endregion Properties

        #region Methods

        public virtual JobAggregator HasJob(ISyncOperation job)
        {
            Jobs.Add(job);
            SubscribeToElement(job);
            return this;
        }

        public virtual ISyncElement DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                RequiredElements.Add(element);

            return this;
        }


        public ICollection<ISyncOperation> Jobs { get; } = new List<ISyncOperation>();

        public override string Name { get; }

        public ICollection<ISyncElement> Dependencies { get; protected set; }
        
        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (ElementData == null)
                throw new InvalidOperationException("ElementData non inizializzato");
        }

        protected override void Clear()
        {
            base.Clear();
        }

        protected virtual void FinalizeSync()
        {
            Clear();
            ElementData.LastUpdate = DateTime.Now;

            RaiseProgressChanged(100);

            try
            {
                SaveElementData();
            }
            catch (Exception e)
            {
                RaiseSyncError(errorMessage: "Impossibile salvare ElementData: " + e.Message + "\t\tInnerException: " + e.InnerException?.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Minor);
            }
            
        }

        protected virtual DateTime GetNextScheduledUpdate()
        {
            return ((DateTime)LastUpdate).AddHours(ElementData.UpdateInterval);
        }
        
        protected virtual void RaiseSyncFailed(Exception e = null)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                ErrorMessage = "Sincronizzazione fallita",
                NameOfElement = Name,
                Severity = SyncErrorEventArgs.ErrorSeverity.Critical,
                TypeOfElement = GetType()
            };

            SyncFailed?.Invoke(this, args);
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