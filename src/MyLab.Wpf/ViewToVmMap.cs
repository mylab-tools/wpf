using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyLab.Wpf
{
    class ViewToVmMap
    {
        readonly IDictionary<string, Type> _viewTypeMap = new Dictionary<string, Type>();

        public void Register(Type viewType, Type vmType)
        {
            _viewTypeMap.Add(GetOriginVmType(vmType).FullName, viewType);
        }

        public Type GetViewType(Type vmType)
        {
            var vmTypeName = GetOriginVmType(vmType).FullName;
            if (!_viewTypeMap.TryGetValue(vmTypeName, out var viewType))
                throw new InvalidOperationException($"Not found view type for view model '{vmTypeName}'");

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