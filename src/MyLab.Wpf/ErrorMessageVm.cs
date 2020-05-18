namespace MyLab.Wpf
{
    public class ErrorMessageVm : DialogVm
    {
        public virtual string Message { get; set; }
        public virtual string HiddenMessage { get; set; }

        public virtual VmCommand Ok { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMessageVm"/>
        /// </summary>
        public ErrorMessageVm()
        {
            Ok = new VmCommand(() => ViewManager.CloseView(this, true));
        }
    }
}
