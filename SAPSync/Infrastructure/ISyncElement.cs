using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface ISyncElement : ISyncBase
    {
        #region Properties

        bool IsUpForScheduledUpdate { get; }
        DateTime? LastUpdate { get; }
        DateTime? NextScheduledUpdate { get; }
        ICollection<ISyncElement> Dependencies { get; }

        #endregion Properties

    }
}