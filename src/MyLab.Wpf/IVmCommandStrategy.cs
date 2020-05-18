namespace MyLab.Wpf
{
    public interface IVmCommandStrategy
    {
        bool CanExecute(object parameter);

        void Execute(object parameter);
    }
}