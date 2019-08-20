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
        private SAPSync _SAPSync;
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
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SAPSyncStartRequested += OnSAPSyncStartRequested;
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SAPSyncStopRequested += OnSAPSyncStopRequested;

            _SAPSync = new SAPSync(_syncManager);

            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _SAPSync.Status.ToString();
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

        private void OnSAPSyncStartRequested(object sender, EventArgs e)
        {
            StartSAPSync();
        }

        private void OnSAPSyncStopRequested(object sender, EventArgs e)
        {
            StopSAPSync();
        }

        private void StartSAPSync()
        {
            _SAPSync.Start();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _SAPSync.Status.ToString();
        }

        private void StopSAPSync()
        {
            _SAPSync.Stop();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _SAPSync.Status.ToString();
        }

        #endregion Methods
    }
}