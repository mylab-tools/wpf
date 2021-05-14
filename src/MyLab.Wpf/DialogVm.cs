namespace MyLab.Wpf
{
    public class DialogVm<TResult> : DialogVm
    {
        public TResult Result { get; private set; }
        
        protected void ClosePositive(TResult result)
        {
            Result = result;
            ClosePositive();
        }
    }

    public class DialogVm : ViewModel
    {
        public IViewManager ViewManager { get; private set; }

        public VmCommand OkCmd { get; private set; }
        public VmCommand NoCmd { get; private set; }
        public VmCommand CancelCmd { get; private set; }

        protected void ClosePositive()
        {
            ViewManager.CloseView(this, true);
        }

        protected void CloseCancel()
        {
            ViewManager.CloseView(this, null);
        }

        protected void CloseNegative()
        {
            ViewManager.CloseView(this, false);
        }

        public void OnClosedPositive()
        {
            OnClosedPositiveCore();
            OnClosedCore();
        }

        public void OnClosedCancel()
        {
            OnClosedCancelCore();
            OnClosedCore();
        }

        public void OnClosedNegative()
        {
            OnClosedNegativeCore();
            OnClosedCore();
        }

        protected virtual void OnClosedPositiveCore() { }
        protected virtual void OnClosedCancelCore() { }
        protected virtual void OnClosedNegativeCore() { }
        protected virtual void OnClosedCore() { }

        protected virtual bool ValidatePositiveClosing() => true;
        protected virtual bool ValidateNegativeClosing() => true;
        protected virtual bool ValidateCancelClosing() => true;

        public override void Initialize(VmInitializationContext ctx)
        {
            base.Initialize(ctx);
            ViewManager = ctx.ViewManager;

            OkCmd = new VmCommand(ClosePositive, ValidatePositiveClosing);
            NoCmd = new VmCommand(CloseNegative, ValidateNegativeClosing);
            CancelCmd = new VmCommand(CloseCancel, ValidateCancelClosing);
        }
    }
}