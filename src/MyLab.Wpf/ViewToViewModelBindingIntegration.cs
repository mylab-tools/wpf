using System;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Wpf
{
    /// <summary>
    /// Provides extensions for <see cref="ServiceCollection"/> to integrate view to viewmodel binding
    /// </summary>
    public static class ViewToViewModelBindingIntegration
    {
        /// <summary>
        /// Adds view to view model binding
        /// </summary>
        public static ServiceCollection AddViewBinding(this ServiceCollection srv,
            Action<ViewBindingRegistrar> bindingRegistrarFunc)
        {
            if (srv == null) throw new ArgumentNullException(nameof(srv));
            if (bindingRegistrarFunc == null) throw new ArgumentNullException(nameof(bindingRegistrarFunc));

            var coll = new Collection<ViewBinding>();
            var bindingRegistrar = new ViewBindingRegistrar(coll);

            bindingRegistrarFunc(bindingRegistrar);

            srv.AddSingleton<IViewToViewModelMap>(new CoreViewToViewModelMap(coll));

            return srv;
        }
    }
}