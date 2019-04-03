using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private SAPReader _reader;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            _reader = new SAPReader();
            InitializeSyncElements();
            StartSyncCommand = new DelegateCommand(() => StartSync());
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand StartSyncCommand { get; set; }

        public ICollection<SyncElement> SyncElements { get; set; }

        #endregion Properties

        #region Methods

        private void InitializeSyncElements()
        {
            try
            {
                SyncElements = _reader.GetSyncElements();
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione elementi di sincronizzazione fallita: " + e.Message);
            }
        }

        private void StartSync()
        {
            try
            {
                foreach (SyncElement syncElement in SyncElements.Where(syel => syel.RequiresSync))
                    _reader.RunSynchronization(syncElement);
            }
            catch (Exception e)
            {
                throw new Exception("Sincronizzazione Fallita:" + e.Message);
            }
        }

        #endregion Methods
    }
}