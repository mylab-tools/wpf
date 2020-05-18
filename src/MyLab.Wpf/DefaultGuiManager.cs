using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MyLab.Wpf
{
    public class DefaultGuiManager : IGuiManager
    {
        private TaskScheduler _uiTaskScheduler;
        private TaskScheduler _bizTaskScheduler;
        private DialogVm _mainVm;
        private Window _mainWindow;

        readonly ViewToVmMap _viewTypeMap = new ViewToVmMap();
        readonly WindowsRegistry _windowsRegistry = new WindowsRegistry();


        public void UiInvoke(Action act)
        {
            var f = new TaskFactory(_uiTaskScheduler);
            f.StartNew(act);
        }

        public void BizInvoke(Action act)
        {
            var f = new TaskFactory(_bizTaskScheduler);
            f.StartNew(act);
        }

        public void BindViewToVm<TView, TViewModel>() 
            where TView : Control, new()
            where TViewModel : ViewModel
        {
            _viewTypeMap.Register(typeof(TView), typeof(TViewModel));
        }

        public void InitApplication([NotNull] Application application, [NotNull] DialogVm mainVm)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            _mainVm = mainVm ?? throw new ArgumentNullException(nameof(mainVm));
            application.Resources.Add("MainViewModel", _mainVm);
            application.Activated += OnApplicationActivated;

            InitTaskScheduler(application.Dispatcher);
            InitBizScheduler();
        }

        public bool? ShowDialog([NotNull] DialogVm viewModel)
        {
            var window = CreateWindowForVm(viewModel);
            _windowsRegistry.Register(window);

            return window.ShowDialog();
        }

        public void Show([NotNull] ViewModel viewModel)
        {
            var window = CreateWindowForVm(viewModel);
            _windowsRegistry.Register(window);

            window.Show();
        }

        public void CloseApplication()
        {
            if(_mainWindow == null)
                throw new InvalidOperationException("Application was not initialized with GUI Manager");

            _mainWindow.Close();
        }

        public void CloseView(DialogVm viewModel, bool? status = null)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            if (!_windowsRegistry.TryGetWindow(viewModel, out var window))
                throw new InvalidOperationException($"View model window not found: '{viewModel}'");

            if (window == _mainWindow)
            {

            }
            else
            {
                window.DialogResult = status;
            }

            window.Close();
        }

        Window CreateWindowForVm(ViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            var windowType = _viewTypeMap.GetViewType(viewModel.GetType());
            
            Window parent = null;
            if (viewModel.Owner != null)
            {
                if (!_windowsRegistry.TryGetWindow(viewModel.Owner, out var parentControl))
                    throw new InvalidOperationException($"Owner window not found. Child view model: '{viewModel.GetType().FullName}', parent view model: '{viewModel.Owner.GetType().FullName}'");
                parent = parentControl;
            }

            var window = (Window)Activator.CreateInstance(windowType);
            window.Owner = parent;

            ApplyWindowVm(window, viewModel);

            return window;
        }

        void ApplyWindowVm(Window window, ViewModel vm)
        {
            window.DataContext = vm;
            var initContext = new VmInitializationContext(this, new ViewProvider(window), _uiTaskScheduler, _bizTaskScheduler);

            if(window.IsInitialized)
                vm.Initialize(initContext);
            else
                window.Initialized += (sender, args) => vm.Initialize(initContext);
        }

        void OnApplicationActivated(object sender, EventArgs args)
        {
            ((Application)sender).Activated -= OnApplicationActivated;

            _mainWindow = ((Application)sender).MainWindow;
            _mainWindow.DataContext = _mainVm;

            _windowsRegistry.Register(_mainWindow);

            ApplyWindowVm(_mainWindow, _mainVm);
        }

        private void InitTaskScheduler(Dispatcher applicationDispatcher)    
        {
            applicationDispatcher.InvokeAsync(() =>
                {
                    _uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

                    Thread.CurrentThread.Name = "GUI";
                }).Wait();

        }

        private void InitBizScheduler()
        {
            Task.Run(() =>
                {
                    Thread.CurrentThread.Name = "BIZ";

                    return _bizTaskScheduler = TaskScheduler.Current;
                })
                .Wait();
        }
    }
}