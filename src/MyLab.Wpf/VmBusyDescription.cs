namespace MyLab.Wpf
{
    public class VmBusyDescription : UiNotifier
    {
        public bool Enabled { get; protected set; }

        public string Message { get; protected set; }

        public void Enable(string message)
        {
            Enabled = true;
            Message = message;

            OnPropertyChanged(nameof(Enabled));
            OnPropertyChanged(nameof(Message));
        }

        public void Disable()
        {
            Enabled = false;
            Message = null;

            OnPropertyChanged(nameof(Enabled));
            OnPropertyChanged(nameof(Message));
        }
    }
}