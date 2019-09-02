using DataAccessCore;
using DMTAgent.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using SSMD;
using System;
using System.IO;
using System.Windows;

namespace DMTAgent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Properties

        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        #endregion Properties

        #region Methods

        protected virtual MySqlDbContextOptionsBuilder GetMySqlDbContextOptions(MySqlDbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.CommandTimeout(1800);

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceProvider.GetService<ISyncManager>().JobController.GetAwaiterForActiveOperations().Wait();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureSettings(serviceCollection);
            ConfigureLogging(serviceCollection);
            ConfigureServices(serviceCollection);
            ConfigureViewModels(serviceCollection);
            ConfigureViews(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            InitializeDb();
            MainWindow = ServiceProvider.GetRequiredService<Views.MainWindow>();
            MainWindow.Show();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            var config = new LoggingConfiguration();
            var target = new LogListener();
            config.AddTarget("LogListener", target);
            config.AddRuleForOneLevel(NLog.LogLevel.Error, target);
            config.AddRuleForOneLevel(NLog.LogLevel.Info, target);
            LogManager.Configuration = config;
            services.AddLogging(cfg => cfg.AddNLog());
            services.AddSingleton<LogListener>(target);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(new LoggerFactory()
            //    .AddNLog());

            services.AddDbContext<SSMDContext>(
                opt => opt.UseMySql(Configuration.GetConnectionString("SSMD"),
                    opt2 => GetMySqlDbContextOptions(opt2)),
                contextLifetime: ServiceLifetime.Transient);

            services.AddTransient(typeof(IDesignTimeDbContextFactory<SSMDContext>), typeof(SSMDContextFactory));
            services.AddTransient(typeof(IDataService<SSMDContext>), typeof(SSMDData));

            services.AddTransient(typeof(ISyncElementFactory), typeof(SyncElementFactory));

            services.AddSingleton(typeof(ISyncManager), typeof(SyncManager));
            services.AddSingleton(typeof(ISyncAgent), typeof(SyncAgent));

            services.AddTransient(typeof(Views.MainWindow));
        }

        private void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
        }

        private void ConfigureViewModels(IServiceCollection services)
        {
            services.AddTransient(
                typeof(IViewModel<Views.MainWindow>),
                typeof(ViewModels.MainWindowViewModel));
        }

        private void ConfigureViews(IServiceCollection services)
        {
            services.AddTransient(typeof(Views.MainWindow));
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