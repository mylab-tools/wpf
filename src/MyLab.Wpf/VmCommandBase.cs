using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace MyLab.Wpf
{
    public class VmCommandBase : ICommand  
    {
        private readonly IVmCommandStrategy _strategy;
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
        /// Initializes a new instance of <see cref="VmCommandBase"/>
        /// </summary>
        protected VmCommandBase( IVmCommandStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public bool CanExecute(object parameter)
        {
            return _strategy.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _strategy.Execute(parameter);
        }
        public void OnCanExecuteChanged()
        {
            _subscribing.CallHandlers();
        }
    }

    public class VmCommand : VmCommandBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VmCommand"/>
        /// </summary>
        public VmCommand(Action act, Func<bool> predicate = null)
            :base(new ParameterlessVmCommandStrategy(act, predicate))
        {
            
        }
    }

    public class ParamVmCommand<TParam> : VmCommandBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ParamVmCommand{TParam}"/>
        /// </summary>
        public ParamVmCommand(Action<TParam> act, Func<TParam, bool> predicate = null)
            : base(new ParameterizedVmCommandStrategy<TParam>(act, predicate))
        {

        }
    }
}
