namespace MyLab.Wpf
{
    /// <summary>
    /// Contains dialog logic
    /// </summary>
    public interface IDialogLogic
    {
        /// <summary>
        /// Calculates whether the dialog can be closed with positive result
        /// </summary>
        bool CanOk(DialogVm dialog);
        /// <summary>
        /// Calculates whether the dialog can be closed with negative result
        /// </summary>
        bool CanCancel(DialogVm dialog);
        /// <summary>
        /// Performs positive action
        /// </summary>
        void Ok(DialogVm dialog);
        /// <summary>
        /// Performs negative action
        /// </summary>
        void Cancel(DialogVm dialog);
    }

    /// <summary>
    /// the dialog logic which do nothing
    /// </summary>
    public class EmptyDialogLogic : IDialogLogic
    {
        public bool CanOk(DialogVm dialog) => true;

        public bool CanCancel(DialogVm dialog) => true;

        public void Ok(DialogVm dialog)
        {
            // do nothing
        }

        public void Cancel(DialogVm dialog)
        {
            // do nothing
        }
    }
}