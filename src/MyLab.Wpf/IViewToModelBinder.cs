using System.Collections.Generic;

namespace MyLab.Wpf
{
    /// <summary>
    /// Gets view to view model bindings
    /// </summary>
    public interface IViewToViewModelMap
    {
        /// <summary>
        /// Gets view to viewmodel bindings
        /// </summary>
        IEnumerable<ViewBinding> GetBinds();
    }

    class CoreViewToViewModelMap : IViewToViewModelMap
    {
        private readonly ICollection<ViewBinding> _bindings;

        /// <summary>
        /// Initializes a new instance of <see cref="CoreViewToViewModelMap"/>
        /// </summary>
        public CoreViewToViewModelMap(ICollection<ViewBinding> bindings)
        {
            _bindings = bindings;
        }

        public IEnumerable<ViewBinding> GetBinds()
        {
            return _bindings;
        }
    }
}
