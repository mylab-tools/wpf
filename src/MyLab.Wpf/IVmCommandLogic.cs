namespace MyLab.Wpf
{
    public interface IVmCommandLogic
    {
        bool CanExecute(object parameter);

        void Execute(object parameter);
    }
}