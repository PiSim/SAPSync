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
        ISyncTaskController SyncTaskController { get; }
        void SyncOutdatedElements();

        #endregion Methods
    }
}