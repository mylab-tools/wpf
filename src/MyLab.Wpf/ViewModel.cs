namespace MyLab.Wpf
{
    public class ViewModel : UiNotifier, IVmStateUpdater
    {
        public string Title { get; set; }

        public ViewModel Owner { get; set; }

        //public VmBusyDescription Busy { get; } = new VmBusyDescription();
        
        /// <summary>
        /// Initializes a new instance of <see cref="ViewModel"/>
        /// </summary>
        protected ViewModel()
        {
            
        }

        void IVmStateUpdater.UpdateStates()
        {
            OnPropertyChanged();
        }
    }
}