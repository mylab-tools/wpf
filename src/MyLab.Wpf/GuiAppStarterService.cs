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
        private readonly IGuiManager _guiMgr;
        private readonly IMainVmProvider _mainVmProvider;
        private readonly Application _application;

        public GuiAppStarterService(IServiceProvider sp, IGuiManager guiMgr, IMainVmProvider mainVmProvider, Application application)
        {
            _sp = sp;
            _guiMgr = guiMgr;
            _mainVmProvider = mainVmProvider;
            _application = application;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var mainVm = _mainVmProvider.Provide(_sp);
            _guiMgr.InitApplication(_application, mainVm);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
