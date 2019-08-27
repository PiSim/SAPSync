using Prism.Mvvm;
using System;

namespace SAPSync.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {
        #region Constructors

        public SyncElementViewModel(ISyncElement syncElement)
        {
            SyncElement = syncElement;
        }

        #endregion Constructors

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