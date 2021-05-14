using System;
using System.Linq.Expressions;
using System.Windows;

namespace MyLab.Wpf
{
    /// <summary>
    /// Creates, shows, close and manage dialogManager
    /// </summary>
    public interface IDialogManager
    {
        bool? ShowDialog(DialogVm dialog);
        void Show(ViewModel viewModel);
        void Close(ViewModel viewModel, bool? status = null);
    }

    class CoreDialogManager : IDialogManager
    {
        private readonly IViewToVmBindsRegistry _viewToVmBindsRegistry;
        readonly WindowsRegistry _windowsRegistry = new WindowsRegistry();

        public CoreDialogManager(IViewToVmBindsRegistry viewToVmBindsRegistry = null)
        {
            _viewToVmBindsRegistry = viewToVmBindsRegistry;
        }

        public void Show(ViewModel viewModel)
        {
            var window = CreateWindowForVm(viewModel);
            _windowsRegistry.Register(window);

            window.Show();
        }

        public bool? ShowDialog(DialogVm dialog)
        {
            var window = CreateWindowForVm(dialog);
            _windowsRegistry.Register(window);

            return window.ShowDialog();
        }

        public void Close(ViewModel viewModel, bool? status = null)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            if (!_windowsRegistry.TryGetWindow(viewModel, out var window))
                throw new InvalidOperationException($"View model window not found: '{viewModel}'");

            window.DialogResult = status;

            window.Close();
        }

        Window CreateWindowForVm(ViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if(_viewToVmBindsRegistry == null) throw new InvalidOperationException("No view bound to models");

            var windowType = _viewToVmBindsRegistry.GetViewType(viewModel.GetType());

            Window parent = null;
            if (viewModel.Owner != null)
            {
                if (!_windowsRegistry.TryGetWindow(viewModel.Owner, out var parentControl))
                    throw new InvalidOperationException($"Owner window not found. Child view model: '{viewModel.GetType().FullName}', parent view model: '{viewModel.Owner.GetType().FullName}'");
                parent = parentControl;
            }

            var windowObj = Activator.CreateInstance(windowType);

            if (windowObj == null)
                throw new InvalidOperationException($"Can create window instance: '{windowType.FullName}'");

            if (!(windowObj is Window window))
                throw new InvalidOperationException($"The window type is not $'{typeof(Window).FullName}' inheritor");

            window.Owner = parent;
            window.DataContext = viewModel;

            return window;
        }
    }
}