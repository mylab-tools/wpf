namespace MyLab.Wpf
{
    public class ErrorMessageVm : DialogVm
    {
        public virtual string Message { get; set; }
        public virtual string HiddenMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMessageVm"/>
        /// </summary>
        public ErrorMessageVm(IDialogCloser dialogCloser)
            :base(new EmptyDialogLogic(), dialogCloser)
        {
        }
    }
}
