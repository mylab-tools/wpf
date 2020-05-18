using System;
using System.Diagnostics.CodeAnalysis;

namespace MyLab.Wpf
{
    class ParameterlessVmCommandStrategy : IVmCommandStrategy
    {
        [NotNull] private readonly Action _act;
        private readonly Func<bool> _predicate;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterlessVmCommandStrategy"/>
        /// </summary>
        public ParameterlessVmCommandStrategy([NotNull] Action act, Func<bool> predicate)
        {
            _act = act ?? throw new ArgumentNullException(nameof(act));
            _predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _act();
        }
    }
}