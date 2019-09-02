using System.Collections.Generic;

namespace DMTAgent.Infrastructure
{
    public interface ISyncElementFactory
    {
        #region Methods

        ICollection<ISyncElement> BuildSyncElements();

        #endregion Methods
    }
}