using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface ISyncElement
    {
        #region Properties

        string Name { get; }
        bool IsUpForScheduledUpdate { get; }
        DateTime? LastUpdate { get; }
        DateTime? NextScheduledUpdate { get; }
        ICollection<ISyncElement> Dependencies { get; }
        ISubJob CurrentJob { get; }

        void Execute(ISubJob newJob);

        #endregion Properties

    }
}