using System;
using System.Collections.Generic;

namespace DMTAgent.Infrastructure
{
    public interface ISyncElement
    {
        #region Properties

        ISubJob CurrentJob { get; }
        ICollection<ISyncElement> Dependencies { get; }
        bool IsUpForScheduledUpdate { get; }
        DateTime? LastUpdate { get; }
        string Name { get; }
        DateTime? NextScheduledUpdate { get; }

        #endregion Properties

        #region Methods

        void Execute(ISubJob newJob);

        #endregion Methods
    }
}