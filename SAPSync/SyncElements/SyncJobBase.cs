using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessCore.Commands;
using SSMD;
using SAPSync;

namespace SAPSync.SyncElements
{
    public abstract class SyncJobBase : SyncElementBase, ISyncJob
    {
        public SyncJobStatus Status { get; protected set; }
                     
        protected override void OnStarting()
        {
            base.OnStarting();
            ChangeStatus(SyncJobStatus.Running);
        }

        protected override void OnCompleting()
        {
            base.OnCompleting();
            ChangeStatus(SyncJobStatus.Completed);
        }

        protected override void OnFailure()
        {
            base.OnFailure();
        }

        protected virtual void ChangeStatus(SyncJobStatus newStatus)
        {
            Status = newStatus;
            RaiseStatusChanged();
        }
    }
}
