using System.Collections.Generic;

namespace SAPSync
{
    public interface INeedsInspectionLots
    {
        #region Properties

        ICollection<int> InspectionLots { set; }

        #endregion Properties
    }
}