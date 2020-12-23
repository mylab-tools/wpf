using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using MyLab.ExpressionTools;

namespace MyLab.Wpf
{
    /// <summary>
    /// Creates view models
    /// </summary>
    public interface IViewModelFactory
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

    public class DesignTimeViewModelFactory : IViewModelFactory
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


    class CoreViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="CoreViewModelFactory"/>
        /// </summary>
        public CoreViewModelFactory(IServiceProvider serviceProvider)
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
