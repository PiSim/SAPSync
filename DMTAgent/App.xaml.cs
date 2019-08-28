using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DMTAgent.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SSMD;

namespace DMTAgent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Fields

        private SyncAgent _DMTAgent;
        private SSMDData _ssData;
        private SyncManager _syncManager;

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        #endregion Fields

        #region Constructors
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureSettings(serviceCollection);
            ConfigureServices(serviceCollection);
            ConfigureViewModels(serviceCollection);
            ConfigureViews(serviceCollection);

            InitializeDb();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).SyncManager = _syncManager;
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).DMTAgentStartRequested += OnDMTAgentStartRequested;
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).DMTAgentStopRequested += OnDMTAgentStopRequested;
            
            MainWindow = ServiceProvider.GetRequiredService<Views.MainWindow>();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _DMTAgent.Status.ToString();
            MainWindow.Show();
        }

        #endregion Constructors

        #region Methods

        private void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(cfg => cfg.AddNLog());
            services.AddSingleton(typeof(SyncManager));
            services.AddSingleton(typeof(SyncAgent));
            services.AddTransient(typeof(Views.MainWindow));
        }
        private void ConfigureViews(IServiceCollection services)
        {
            services.AddTransient(typeof(Views.MainWindow));
        }

        private void ConfigureViewModels(IServiceCollection services)
        {
            services.AddTransient(
                typeof(IViewModel<Views.MainWindow>),
                typeof(ViewModels.MainWindowViewModel));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _syncManager.JobController.GetAwaiterForActiveOperations().Wait();
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

        private void OnDMTAgentStartRequested(object sender, EventArgs e)
        {
            StartDMTAgent();
        }

        private void OnDMTAgentStopRequested(object sender, EventArgs e)
        {
            StopDMTAgent();
        }

        private void StartDMTAgent()
        {
            _DMTAgent.Start();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _DMTAgent.Status.ToString();
        }

        private void StopDMTAgent()
        {
            _DMTAgent.Stop();
            (MainWindow.DataContext as ViewModels.MainWindowViewModel).ServiceStatus = _DMTAgent.Status.ToString();
        }

        #endregion Methods
    }
}
