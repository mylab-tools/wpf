using System;

namespace MyLab.Wpf
{
    /// <summary>
    /// Determines binding form View to ViewModel
    /// </summary>
    public class ViewBinding
    {
        /// <summary>
        /// View type
        /// </summary>
        public Type View { get; }
        /// <summary>
        /// ViewModel type
        /// </summary>
        public Type ViewModel { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ViewBinding"/>
        /// </summary>
        public ViewBinding(Type viewModel, Type view)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            View = view ?? throw new ArgumentNullException(nameof(view));
        }
    }
}