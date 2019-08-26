using System;
using System.Collections.Generic;

namespace SAPSync
{
    public interface ISyncManager
    {
        #region Properties

        IJobController JobController { get; }

        #endregion Properties

        #region Methods

        IEnumerable<DateTime?> GetUpdateSchedule();

        void StartSync(IEnumerable<ISyncElement> syncElements);

        void SyncOutdatedElements();

        #endregion Methods
    }
}