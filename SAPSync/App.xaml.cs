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

        #endregion Fields

        #region Constructors

        public App() : base()
        {
            InitSAP();
            InitializeDb();
            SAPReader reader = new SAPReader();
            MainWindow = new Views.MainWindow();
            MainWindow.Show();
        }

        #endregion Constructors

        #region Methods

        public void InitSAP()
        {
            string destinationConfigName = "PRD";
            IDestinationConfiguration destinationConfig = null;
            bool destinationIsInitialized = false;
            if (!destinationIsInitialized)
            {
                destinationConfig = new SAPDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);

                bool destinationFound = false;

                try
                {
                    destinationFound = (RfcDestinationManager.GetDestination(destinationConfigName) != null);
                }
                catch (Exception e)
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

        private void InitializeDb()
        {
            try
            {
                _ssData = new SSMDData(new SSMDContextFactory());
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione Database Fallita");
            }
        }

        #endregion Methods
    }
}