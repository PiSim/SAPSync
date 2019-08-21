using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface ISyncManager
    {
        #region Methods

        IEnumerable<DateTime?> GetUpdateSchedule();
        void StartSync(IEnumerable<ISyncElement> syncElements);
        IJobController JobController { get; }
        void SyncOutdatedElements();

        #endregion Methods
    }
}