using System;
using System.Linq.Expressions;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyLab.Wpf
{
    /// <summary>
    /// Provides methods to initialize GUI application
    /// </summary>
    public class GuiApp
    {
        private readonly Application _app;
        private IAppServiceConfigurator _serviceConfigurator;
        private IAppConfigDescriptor _configDescriptor;
        
        GuiApp(Application app)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
        }

        GuiApp(GuiApp initial)
        {
            _serviceConfigurator = initial._serviceConfigurator;
            _configDescriptor = initial._configDescriptor;
            _app = initial._app;
        }

        public static GuiApp Create(Application app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return new GuiApp(app);
        }

        /// <summary>
        /// Specifies configuration descriptor which defines config sources
        /// </summary>
        public GuiApp UseConfiguration(IAppConfigDescriptor configDescriptor)
        {
            if (configDescriptor == null) throw new ArgumentNullException(nameof(configDescriptor));

            return new GuiApp(this)
            {
                _configDescriptor = configDescriptor
            };
        }

        /// <summary>
        /// Specifies service configurator which build service collection
        /// </summary>
        public GuiApp UseServiceConfiguration(IAppServiceConfigurator serviceConfigurator)
        {
            if (serviceConfigurator == null) throw new ArgumentNullException(nameof(serviceConfigurator));

            return new GuiApp(this)
            {
                _serviceConfigurator = serviceConfigurator
            };
        }

        /// <summary>
        /// Starts app logic with specified main view model
        /// </summary>
        public void Start<TMainVm>()
            where TMainVm : DialogVm
        {
            CoreStart(new GenericMainVmProvider<TMainVm>());
        }

        /// <summary>
        /// Starts app logic with specified main view model
        /// </summary>
        public void Start<TMainVm>(Expression<Func<TMainVm>> factoryExpr)
            where TMainVm : DialogVm
        {
            if (factoryExpr == null) throw new ArgumentNullException(nameof(factoryExpr));

            CoreStart(new LambdaMainVmProvider<TMainVm>(factoryExpr));
        }

        /// <summary>
        /// Starts app logic with specified main view model
        /// </summary>
        public void Start(DialogVm mainVm)
        {
            if (mainVm == null) throw new ArgumentNullException(nameof(mainVm));

            CoreStart(new SingleInstanceMainVmProvider(mainVm));
        }

        void CoreStart(IMainVmProvider mainVmProvider)
        {
            var hb = new HostBuilder();

            if (_configDescriptor != null)
                hb.ConfigureAppConfiguration(_configDescriptor.Describe);
            
            hb.ConfigureServices((context, collection) =>
            {
                var uiInvoker = CoreUiInvoker.CreateAndInitThreads(_app.Dispatcher);

                collection.AddSingleton<IUiInvoker>(uiInvoker);
                collection.AddSingleton<IDialogManager, CoreDialogManager>();
                collection.AddSingleton<IDialogCloser, DialogManagerCloser>();
                collection.AddSingleton<IVmFactory, CoreVmFactory>();
                collection.AddSingleton(mainVmProvider);
                collection.AddSingleton(_app);
                collection.AddHostedService<GuiAppStarterService>();

                _serviceConfigurator?.Configure(context.Configuration, collection);
            });

            var host = hb.Build();

            _app.Exit += (sender, args) =>
            {
                host.StopAsync().Wait();
                host.Dispose();
            };
            
            host.Start();
        }
    }
}
