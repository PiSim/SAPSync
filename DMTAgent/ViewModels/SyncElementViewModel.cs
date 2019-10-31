using DMTAgent.Infrastructure;
using System;

namespace DMTAgent.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {
        #region Constructors

        public SyncElementViewModel(ISyncElement syncElement)
        {
            SyncElement = syncElement;
        }

        #endregion Constructors

        public void RaiseChange()
        {
            RaisePropertyChanged("LastUpdate");
            RaisePropertyChanged("NextScheduledUpdate");
        }

        #region Properties
        
        public bool IsSelected { get; set; }
        public bool IsUpdateForbidden { get; set; }
        public DateTime? LastUpdate => SyncElement.LastUpdate;
        public string Name => SyncElement?.Name;
        public DateTime? NextScheduledUpdate => SyncElement.NextScheduledUpdate;
        public ISyncElement SyncElement { get; }

        #endregion Properties
    }
}