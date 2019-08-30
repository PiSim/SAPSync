using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DataAccessCore;
using DMTAgent.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    public partial class App : Application
    {
        #region Fields

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
            ServiceProvider = serviceCollection.BuildServiceProvider();

            InitializeDb();
            MainWindow = ServiceProvider.GetRequiredService<Views.MainWindow>();
            MainWindow.Show();
        }

        #endregion Constructors

        #region Methods

        private void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        }

        protected virtual MySqlDbContextOptionsBuilder GetMySqlDbContextOptions(MySqlDbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.CommandTimeout(1800);


        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(cfg => cfg.AddNLog());

            services.AddDbContext<SSMDContext>(
                opt => opt.UseMySql(Configuration.GetConnectionString("SSMD"),
                    opt2 => GetMySqlDbContextOptions(opt2)));

            services.AddTransient(typeof(IDesignTimeDbContextFactory<SSMDContext>), typeof(SSMDContextFactory));
            services.AddTransient(typeof(IDataService<SSMDContext>), typeof(SSMDData));

            services.AddTransient(typeof(ISyncElementFactory), typeof(SyncElementFactory));

            services.AddSingleton(typeof(ISyncManager), typeof(SyncManager));
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
            ServiceProvider.GetService<ISyncManager>().JobController.GetAwaiterForActiveOperations().Wait();
            base.OnExit(e);
        }

        private void InitializeDb()
        {
            try
            {
                // TODO -  Check DB exists
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
            ServiceProvider.GetService<SyncAgent>().Start();
        }

        private void StopDMTAgent()
        {
            ServiceProvider.GetService<SyncAgent>().Stop();
        }

        #endregion Methods
    }
}
