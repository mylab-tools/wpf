using System;
using System.Windows;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    public interface IGuiManager : IViewManager, IUiInvoker
    {
        
        void BindViewToVm<TView, TViewModel>() 
            where TView : Control, new()
            where TViewModel : ViewModel;

        void BindViewToVm(ViewBinding binding);

        void InitApplication(Application application, DialogVm mainVm);

        void CloseApplication();
    }

    public interface IViewManager
    {
        bool? ShowDialog(DialogVm viewModel);
        void Show(ViewModel viewModel);

        void CloseView(DialogVm viewModel, bool? status = null);
    }

    public interface IUiInvoker
    {
        void UiInvoke(Action act);
        void BizInvoke(Action act);
    }
}
