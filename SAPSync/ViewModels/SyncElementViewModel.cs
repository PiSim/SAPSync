using Prism.Mvvm;
using SAPSync.SyncElements;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAPSync.ViewModels
{
    public class SyncElementViewModel : BindableBase
    {

        #region Fields

        #endregion Fields

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
        public DateTime? NextScheduledUpdate => SyncElement.NextScheduledUpdate;
        public ISyncElement SyncElement { get; }

        #endregion Properties

        #region Methods


        #endregion Methods
    }
}