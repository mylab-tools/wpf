using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ExpressionTools;

namespace MyLab.Wpf
{
    /// <summary>
    /// Creates view models
    /// </summary>
    public interface IVmFactory
    {
        /// <summary>
        /// Creates instance of specified view model type
        /// </summary>
        TViewModel Create<TViewModel>() where TViewModel : ViewModel;

        /// <summary>
        /// Creates instance of specified view model type
        /// </summary>
        TViewModel Create<TViewModel>(Expression<Func<TViewModel>> createExpr) where TViewModel : ViewModel;
    }

    /// <summary>
    /// Extensions for <see cref="IVmFactory"/>
    /// </summary>
    public static class VmFactoryExtensions
    {
        /// <summary>
        /// Creates instance of specified view model type
        /// </summary>
        public static TViewModel CreateChild<TViewModel>(this IVmFactory vmFactory, ViewModel owner) where TViewModel : ViewModel
        {
            if (vmFactory == null) throw new ArgumentNullException(nameof(vmFactory));
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            var child = vmFactory.Create<TViewModel>();
            child.Owner = owner;
            return child;
        }

        /// <summary>
        /// Creates instance of specified view model type
        /// </summary>
        public static TViewModel CreateChild<TViewModel>(this IVmFactory vmFactory, ViewModel owner, Expression<Func<TViewModel>> createExpr)
            where TViewModel : ViewModel
        {
            if (vmFactory == null) throw new ArgumentNullException(nameof(vmFactory));
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            var child = vmFactory.Create(createExpr);
            child.Owner = owner;
            return child;
        }
    }

    public class DesignTimeVmFactory : IVmFactory
    {
        public TViewModel Create<TViewModel>() where TViewModel : ViewModel
        {
            var wrapperType = ViewModelTypeWrapperBuilder.RetrieveVmTypeWrapper(typeof(TViewModel));

            return (TViewModel)Activator.CreateInstance(wrapperType);
        }

        public TViewModel Create<TViewModel>(Expression<Func<TViewModel>> createExpr) where TViewModel : ViewModel
        {
            if (createExpr == null) throw new ArgumentNullException(nameof(createExpr));

            var wrapperType = ViewModelTypeWrapperBuilder.RetrieveVmTypeWrapper(typeof(TViewModel));

            var expr = FactoryExpressionTypeReplacer.Replace(createExpr, wrapperType);

            return expr.Body.GetValue<TViewModel>();
        }
    }


    class CoreVmFactory : IVmFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="CoreVmFactory"/>
        /// </summary>
        public CoreVmFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TViewModel Create<TViewModel>() where TViewModel : ViewModel
        {
            var wrapperType = ViewModelTypeWrapperBuilder.RetrieveVmTypeWrapper(typeof(TViewModel));

            return (TViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, wrapperType);
        }

        public TViewModel Create<TViewModel>(Expression<Func<TViewModel>> createExpr) where TViewModel : ViewModel
        {
            if (createExpr == null) throw new ArgumentNullException(nameof(createExpr));

            var wrapperType = ViewModelTypeWrapperBuilder.RetrieveVmTypeWrapper(typeof(TViewModel));

            var expr = FactoryExpressionTypeReplacer.Replace(createExpr, wrapperType);

            return expr.Body.GetValue<TViewModel>();
        }
    }
}
