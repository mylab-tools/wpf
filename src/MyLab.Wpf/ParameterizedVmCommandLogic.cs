using System;

namespace MyLab.Wpf
{
    class ParameterizedVmCommandLogic<T> : IVmCommandLogic
    {
        private readonly Action<T> _act;
        private readonly Func<T, bool> _predicate;

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterizedVmCommandLogic{T}"/>
        /// </summary>
        public ParameterizedVmCommandLogic( Action<T> act, Func<T, bool> predicate)
        {
            _act = act ?? throw new ArgumentNullException(nameof(act));
            _predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _act((T)parameter);
        }
    }
}