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
        private readonly IViewToViewModelMap _viewToViewModelMap;

        public GuiAppStarterService(
            IServiceProvider sp, 
            IGuiManager guiMgr, 
            IMainVmProvider mainVmProvider, 
            Application application,
            IViewToViewModelMap viewToViewModelMap = null)
        {
            _sp = sp;
            _guiMgr = guiMgr;
            _mainVmProvider = mainVmProvider;
            _application = application;
            _viewToViewModelMap = viewToViewModelMap;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var mainVm = _mainVmProvider.Provide(_sp);
            _guiMgr.InitApplication(_application, mainVm);

            if (_viewToViewModelMap != null)
            {
                foreach (var binding in _viewToViewModelMap.GetBinds())
                {
                    _guiMgr.BindViewToVm(binding);
                }
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
