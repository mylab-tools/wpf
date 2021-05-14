using System;
using System.Windows.Input;

namespace MyLab.Wpf
{
    public class VmCommand : ICommand  
    {
        private readonly IVmCommandLogic _logic;
        private readonly VmCommandSubscribing _subscribing = new VmCommandSubscribing();

        public event EventHandler CanExecuteChanged
        {
            add => _subscribing.AddHandler(value);
            remove => _subscribing.RemoveHandler(value);
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="VmCommand"/>
        /// </summary>
        public VmCommand(IVmCommandLogic logic)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        }

        public VmCommand(Action act, Func<bool> predicate = null)
            : this(new ParameterlessVmCommandLogic(act, predicate))
        {
        }

        public bool CanExecute(object parameter)
        {
            return _logic.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _logic.Execute(parameter);
        }
        public void OnCanExecuteChanged()
        {
            _subscribing.CallHandlers();
        }
    }
}
