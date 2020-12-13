using System.Collections.Generic;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    /// <summary>
    /// Registers view to view model bindings
    /// </summary>
    public class ViewBindingRegistrar
    {
        private readonly ICollection<ViewBinding> _bindings;

        /// <summary>
        /// Initializes a new instance of <see cref="ViewBindingRegistrar"/>
        /// </summary>
        public ViewBindingRegistrar(ICollection<ViewBinding> bindings)
        {
            _bindings = bindings;
        }

        public void Add<TView, TViewModel>()
            where TView : Control, new()
            where TViewModel : ViewModel
        {
            _bindings.Add(new ViewBinding
            {
                View = typeof(TView),
                ViewModel = typeof(TViewModel)
            });
        }
    }
}