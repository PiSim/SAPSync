using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements
{
    public abstract class SyncOperationBase : SyncElementBase, ISyncOperation
    {
        public SyncOperationStatus Status { get; protected set; }
                     
        protected override void OnStarting()
        {
            base.OnStarting();
            ChangeStatus(SyncOperationStatus.Running);
        }

        protected override void OnCompleting()
        {
            base.OnCompleting();
            ChangeStatus(SyncOperationStatus.Completed);
        }

        protected virtual void ChangeStatus(SyncOperationStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }
    }
}
