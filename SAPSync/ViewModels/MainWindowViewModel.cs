using Prism.Commands;
using Prism.Mvvm;
using SAPSync.SyncElements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private SyncManager _syncManager;

        #endregion Fields

        #region Constructors

        public MainWindowViewModel()
        {
            _syncManager = new SyncManager();
            StartSyncCommand = new DelegateCommand(() => StartSync());
            GetSyncElements();
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand StartSyncCommand { get; set; }

        public List<SyncElementViewModel> SyncElements { get; set; }

        #endregion Properties

        #region Methods

        public void GetSyncElements()
        {
            SyncElements = new List<SyncElementViewModel>();

            foreach (ISyncElement syncElement in _syncManager.SyncElements)
            {
                SyncElementViewModel newVM = new SyncElementViewModel()
                {
                    SyncElement = syncElement
                };
                syncElement.ProgressChanged += newVM.OnPhaseProgressChanged;
                syncElement.StatusChanged += newVM.OnStatusChanged;

                SyncElements.Add(newVM);
            }
        }

        private async void StartSync()
        {
            await Task.Run(() => _syncManager.StartSync());
            return;
        }

        #endregion Methods
    }
}