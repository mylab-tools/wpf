using System;

namespace MyLab.Wpf
{
    /// <summary>
    /// Closes dialog
    /// </summary>
    public interface IDialogCloser
    {
        /// <summary>
        /// Closes dialog with result status
        /// </summary>
        void Close(DialogVm dialog, bool? status);
    }

    class DialogManagerCloser : IDialogCloser
    {
        private readonly IDialogManager _dialogManager;

        public DialogManagerCloser(IDialogManager dialogManager)
        {
            _dialogManager = dialogManager ?? throw new ArgumentNullException(nameof(dialogManager));
        }

        public void Close(DialogVm dialog, bool? status)
        {
            if (dialog == null) throw new ArgumentNullException(nameof(dialog));
            _dialogManager.Close(dialog, status);
        }
    }
}
