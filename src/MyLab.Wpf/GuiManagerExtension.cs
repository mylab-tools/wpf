using System;

namespace MyLab.Wpf
{
    /// <summary>
    /// Provides abilities to work with standard dialogs
    /// </summary>
    public class StandardDialogs
    {
        private readonly IVmFactory _vmFactory;
        private readonly IDialogManager _dialogManager;
        private readonly ViewModel _owner;

        /// <summary>
        /// Initializes a new instance of <see cref="StandardDialogs"/>
        /// </summary>
        public StandardDialogs(IVmFactory vmFactory, IDialogManager dialogManager, ViewModel owner)
        {
            _vmFactory = vmFactory ?? throw new ArgumentNullException(nameof(vmFactory));
            _dialogManager = dialogManager ?? throw new ArgumentNullException(nameof(dialogManager));
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public void ShowError(string message, string hiddenMsg = null)
        {
            var msgVm = _vmFactory.CreateChild<ErrorMessageVm>(_owner);
            msgVm.HiddenMessage = hiddenMsg;
            msgVm.Message = message;

            _dialogManager.ShowDialog(msgVm);
        }

        public bool? ShowQuestion(string message, IDialogLogic logic)
        {
            var msgVm = _vmFactory.CreateChild(_owner, () => new QuestionMessageVm());
            msgVm.Message = message;

            return _dialogManager.ShowDialog(msgVm);
        }
    }
}
