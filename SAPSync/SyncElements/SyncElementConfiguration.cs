using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{

    public class SyncElementConfiguration
    {
        #region Properties

        public bool ContinueExportingOnImportFail { get; set; } = false;

        #endregion Properties
    }
}
