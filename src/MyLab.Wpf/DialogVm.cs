using System;
using System.Linq.Expressions;

namespace MyLab.Wpf
{
    /// <summary>
    /// Determines view model with dialog features
    /// </summary>
    public class DialogVm : ViewModel//, IVmFactoryBindable, IDialogManagerBindable
    {
        private readonly VmCommandRegistry _commandRegistry = new VmCommandRegistry();
        
        /// <summary>
        /// Positive completion command
        /// </summary>
        public VmCommand OkCmd { get; }
        /// <summary>
        /// Negative completion command
        /// </summary>
        public VmCommand CancelCmd { get; }

        /// <summary>
        /// Dialog logic
        /// </summary>
        protected IDialogLogic DialogLogic { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DialogVm"/>
        /// </summary>
        public DialogVm(IDialogLogic dialogLogic, IDialogCloser closer)
        {
            if (closer == null) throw new ArgumentNullException(nameof(closer));
            DialogLogic = dialogLogic ?? throw new ArgumentNullException(nameof(dialogLogic));

            OkCmd = CreateCommand(() =>
            {
                dialogLogic.Ok(this);
                closer.Close(this, true);
            }, () => dialogLogic.CanOk(this));
            CancelCmd = CreateCommand(() =>
            {
                dialogLogic.Cancel(this);
                closer.Close(this, null);
            }, () => dialogLogic.CanCancel(this));
        }

        /// <summary>
        /// Creates child view-model
        /// </summary>
        public T CreateChildVm<T>(IVmFactory vmFactory)
            where T : ViewModel 
        {
            var vm = vmFactory.Create<T>();
            vm.Owner = this;

            return vm;
        }

        /// <summary>
        /// Creates child view-model
        /// </summary>
        public T CreateChildVm<T>(IVmFactory vmFactory, Expression<Func<T>> createExpr)
            where T : ViewModel
        {
            var vm = vmFactory.Create(createExpr);
            vm.Owner = this;

            return vm;
        }

        /// <summary>
        /// Create view-model command
        /// </summary>
        /// <param name="logic">command logic</param>
        /// <returns>created view-model command</returns>
        protected VmCommand CreateCommand(IVmCommandLogic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            var cmd = new VmCommand(logic);

            _commandRegistry.RegisterCommand(cmd);

            return cmd;
        }

        /// <summary>
        /// Create view-model command
        /// </summary>
        /// <param name="act">command action logic</param>
        /// <param name="predicate">command prediction logic</param>
        /// <returns>created view-model command</returns>
        protected VmCommand CreateCommand<TParam>(Action<TParam> act, Func<TParam, bool> predicate = null)
        {
            return CreateCommand(new ParameterizedVmCommandLogic<TParam>(act, predicate));
        }

        /// <summary>
        /// Create view-model command
        /// </summary>
        /// <param name="act">command action logic</param>
        /// <param name="predicate">command prediction logic</param>
        /// <returns>created view-model command</returns>
        protected VmCommand CreateCommand(Action act, Func<bool> predicate = null)
        {
            return CreateCommand(new ParameterlessVmCommandLogic(act, predicate));
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            _commandRegistry.UpdateStates();
        }
    }
}