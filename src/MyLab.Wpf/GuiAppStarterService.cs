using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace MyLab.Wpf
{
    class GuiAppStarterService : IHostedService
    {
        private readonly IServiceProvider _sp;
        private readonly IMainVmProvider _mainVmProvider;
        private readonly Application _application;
        private DialogVm _mainVm;

        public GuiAppStarterService(
            IServiceProvider sp, 
            IMainVmProvider mainVmProvider, 
            Application application)
        {
            _sp = sp;
            _mainVmProvider = mainVmProvider;
            _application = application ?? throw new ArgumentNullException(nameof(application));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _mainVm = _mainVmProvider.Provide(_sp);

            _application.Resources.Add("MainViewModel", _mainVm);
            _application.Activated += OnApplicationActivated;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        void OnApplicationActivated(object sender, EventArgs args)
        {
            _application.Activated -= OnApplicationActivated;

            if (_application.MainWindow == null)
                throw new InvalidOperationException("Main windows not found aster application activation");

            _application.MainWindow.DataContext = _mainVm;
        }
    }
}
