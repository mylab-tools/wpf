using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Wpf
{
    /// <summary>
    /// Provides main view model instance
    /// </summary>
    public interface IMainVmProvider
    {
        /// <summary>
        /// Provides view model
        /// </summary>
        DialogVm Provide(IServiceProvider sp);
    }

    /// <summary>
    /// Providers precreated single main view model instance
    /// </summary>
    public class SingleInstanceMainVmProvider : IMainVmProvider
    {
        private readonly DialogVm _mainVm;

        /// <summary>
        /// Initializes a new instance of <see cref="SingleInstanceMainVmProvider"/>
        /// </summary>
        public SingleInstanceMainVmProvider(DialogVm mainVm)
        {
            _mainVm = mainVm ?? throw new ArgumentNullException(nameof(mainVm));
        }

        public DialogVm Provide(IServiceProvider sp)
        {
            return _mainVm;
        }
    }

    /// <summary>
    /// Provides specified view model 
    /// </summary>
    public class GenericMainVmProvider<TMainVm> : IMainVmProvider
        where TMainVm : DialogVm
    {
        public DialogVm Provide(IServiceProvider sp)
        {
            var vmFactory = sp.GetService<IViewModelFactory>();
            return vmFactory.Create<TMainVm>();
        }
    }

    /// <summary>
    /// Provides view model which created with specified expression
    /// </summary>
    public class LambdaMainVmProvider<TMainVm> : IMainVmProvider
        where TMainVm : DialogVm
    {
        private readonly Expression<Func<TMainVm>> _factoryExpr;

        /// <summary>
        /// Initializes a new instance of <see cref="LambdaMainVmProvider{TMainVm}"/>
        /// </summary>
        public LambdaMainVmProvider(Expression<Func<TMainVm>> factoryExpr)
        {
            _factoryExpr = factoryExpr;
        }

        public DialogVm Provide(IServiceProvider sp)
        {
            var vmFactory = sp.GetService<IViewModelFactory>();
            return vmFactory.Create<TMainVm>(_factoryExpr);
        }
    }
}
