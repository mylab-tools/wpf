namespace MyLab.Wpf
{
    public class QuestionMessageVm : DialogVm
    {

        public virtual string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMessageVm"/>
        /// </summary>
        public QuestionMessageVm(IDialogLogic logic, IDialogCloser closer)
            :base(logic, closer)
        {
        }
    }
}