using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MyLab.Wpf
{
    /// <summary>
    /// Gets view to view model bindings
    /// </summary>
    public interface IViewToVmBindsRegistry
    {
        /// <summary>
        /// Gets view type for view-model type
        /// </summary>
        /// <exception cref="InvalidOperationException">View type not bound</exception>
        Type GetViewType(Type vmType);
    }

    class CoreViewToVmBindsRegistry : IViewToVmBindsRegistry
    {
        private readonly IDictionary<string, Type> _viewTypeMap = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of <see cref="CoreViewToVmBindsRegistry"/>
        /// </summary>
        public CoreViewToVmBindsRegistry(IEnumerable<ViewBinding> bindings)
        {
            foreach (var viewBinding in bindings)
            {
                var vmType = GetOriginVmType(viewBinding.ViewModel);
                if(vmType == null)
                    throw new InvalidOperationException($"Can't detect view-model type for: '{viewBinding.ViewModel.FullName}'");
                if (vmType.FullName == null)
                    throw new InvalidOperationException($"View-model type has no full name. Type guid: '{vmType.GUID}'");
                
                _viewTypeMap.Add(vmType.FullName, viewBinding.View);
            }
        }

        /// <summary>
        /// Gets view type for view-model type
        /// </summary>
        /// <exception cref="InvalidOperationException">View type not bound</exception>
        public Type GetViewType(Type vmType)
        {
            var vmTypeName = GetOriginVmType(vmType).FullName;

            if (vmTypeName == null)
                throw new InvalidOperationException($"View-model type has no full name. Type guid: '{vmType.GUID}'");

            if (!_viewTypeMap.TryGetValue(vmTypeName, out var viewType))
                throw new InvalidOperationException($"No view type bound for view-model type '{vmTypeName}'");

            return viewType;
        }

        Type GetOriginVmType(Type vmType)
        {
            return vmType.CustomAttributes.Any(a => a.AttributeType == typeof(IsVmWrapperAttribute))
                ? GetOriginVmType(vmType.BaseType)
                : vmType;
        }
    }
}
