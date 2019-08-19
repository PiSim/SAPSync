using System;

namespace SSMD
{
    public class SyncElementData
    {
        public SyncElementData()
        {
            UpdateInterval = 4;
        }

        #region Properties

        public string ElementType { get; set; }
        public int ID { get; set; }
        public DateTime LastUpdate { get; set; }
        public int UpdateInterval { get; set; }

        #endregion Properties
    }
}