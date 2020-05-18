namespace MyLab.Wpf
{
    public static class GuiManagerExtension
    {
        public static void ShowError(this IViewManager gui, DialogVm ownerVm, string message, string hiddenMsg = null, string title = null)
        {
            var msgVm = ownerVm.CreateChild<ErrorMessageVm>();
            msgVm.HiddenMessage = hiddenMsg;
            msgVm.Message = message;
            msgVm.Title = title ?? "Error";

            gui.ShowDialog(msgVm);
        }

        public static bool? ShowQuestion(this IViewManager gui, DialogVm ownerVm, string message, string title = null)
        {
            var msgVm = ownerVm.CreateChild<QuestionMessageVm>();
            msgVm.Message = message;
            msgVm.Title = title ?? "Warning";

            return gui.ShowDialog(msgVm);
        }
    }
}
