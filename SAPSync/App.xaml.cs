using SAP.Middleware.Connector;
using SSMD;
using System;
using System.Windows;

namespace SAPSync
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private SSMDData _ssData;
        private SyncManager _syncManager;
        private SyncService.SyncService _syncService;
        private bool destinationIsInitialized = false;

        #endregion Fields

        #region Constructors

        public App() : base()
        {
            InitSAP();
            InitializeDb();
            SAPReader reader = new SAPReader();
            MainWindow = new Views.MainWindow();
            _syncManager = new SyncManager();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SyncManager = _syncManager;
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SyncServiceStartRequested += OnSyncServiceStartRequested;
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SyncServiceStopRequested += OnSyncServiceStopRequested;

            _syncService = new SyncService.SyncService(_syncManager);

            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _syncService.Status.ToString();
            MainWindow.Show();
        }

        #endregion Constructors

        #region Methods

        public void InitSAP()
        {
            string destinationConfigName = "PRD";
            IDestinationConfiguration destinationConfig = null;
            if (!destinationIsInitialized)
            {
                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);

                bool destinationFound = false;

                try
                {
                    destinationFound = (RfcDestinationManager.GetDestination(destinationConfigName) != null);
                }
                catch
                {
                    destinationFound = false;
                }

                if (!destinationFound)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationIsInitialized = true;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _syncManager.SyncTaskController.GetAwaiterForOpenReadTasks().Wait();
            base.OnExit(e);
        }

        private void InitializeDb()
        {
            try
            {
                _ssData = new SSMDData(new SSMDContextFactory());
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione Database Fallita: " + e.Message, e);
            }
        }

        private void OnSyncServiceStartRequested(object sender, EventArgs e)
        {
            StartSyncService();
        }

        private void OnSyncServiceStopRequested(object sender, EventArgs e)
        {
            StopSyncService();
        }

        private void StartSyncService()
        {
            _syncService.Start();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _syncService.Status.ToString();
        }

        private void StopSyncService()
        {
            _syncService.Stop();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _syncService.Status.ToString();
        }

        #endregion Methods
    }
}