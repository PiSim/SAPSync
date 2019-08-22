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
    public class OperationAggregator : SyncElementBase
    {

        public OperationAggregator(string name = "", SyncElementConfiguration configuration = null) : base(name, configuration)
        {

        }

        #region Constructors
                
        public event EventHandler SyncAborted;

        public event EventHandler SyncCompleted;

        public event EventHandler<SyncErrorEventArgs> SyncFailed;

        #endregion Events

        #region Properties


        #endregion Properties

        #region Methods

        public virtual OperationAggregator HasOperation(ISyncOperation newOperation)
        {
            Operations.Add(newOperation);
            newOperation.SetParent(this);
            return this;
        }

        public virtual ISyncElement DependsOn(IEnumerable<ISyncElement> parentElements)
        {
            foreach (ISyncElement element in parentElements)
                Dependencies.Add(element);

            return this;
        }


        public ICollection<ISyncOperation> Operations { get; } = new List<ISyncOperation>();


        protected virtual void FinalizeSync()
        {
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


        #endregion Methods

    }
}