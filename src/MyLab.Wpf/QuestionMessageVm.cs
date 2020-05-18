namespace MyLab.Wpf
{
    public class QuestionMessageVm : DialogVm
    {
        public virtual string Message { get; set; }

        public virtual VmCommand Ok { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorMessageVm"/>
        /// </summary>
        public QuestionMessageVm()
        {
            Ok = new VmCommand(() => ViewManager.CloseView(this, true));
        }
    }
}