using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyLab.Wpf
{
    static class ViewModelExpressionValueProvidingTools
    {
        private static readonly IExpressionValueProvider[] ValueProviders =
        {
            new ConstantExpressionValueProvider(),
            new MemberExpressionValueProvider(),
            new CallExpressionValueProvider(),
            new NewExpressionValueProvider(),
            new MemberInitExpressionValueProvider(),
            new ConvertExpressionValueProvider(),
            new LambdaExpressionValueProvider()
        };

        public static object GetValue(Expression expression)
        {
            var valProvider = ValueProviders.FirstOrDefault(p => p.Predicate(expression));

            if (valProvider == null)
                throw new NotSupportedException($"Expression type '{expression.NodeType}' not supported");

            return valProvider.GetValue(expression);
        }
    }

    internal class LambdaExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Lambda;
        }

        public object GetValue(Expression expression)
        {
            var lambda = (LambdaExpression) expression;
            return ViewModelExpressionValueProvidingTools.GetValue(lambda.Body);
        }
    }

    internal class ConvertExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.Quote;
        }

        public object GetValue(Expression expression)
        {
            var uExpr = (UnaryExpression)expression;
            return ViewModelExpressionValueProvidingTools.GetValue(uExpr.Operand);
        }
    }

    interface IExpressionValueProvider
    {
        bool Predicate(Expression expression);

        object GetValue(Expression expression);
    }

    class ConstantExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression == null || expression.NodeType == ExpressionType.Constant;
        }

        public object GetValue(Expression expression)
        {
            return ((ConstantExpression)expression)?.Value;
        }
    }

    class MemberExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        public object GetValue(Expression expression)
        {
            var ma = (MemberExpression)expression;

            var targetObject = ViewModelExpressionValueProvidingTools.GetValue(ma.Expression);

            switch (ma.Member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)ma.Member).GetValue(targetObject);
                case MemberTypes.Method:
                    return ((PropertyInfo)ma.Member).GetValue(targetObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)ma.Member).GetValue(targetObject);
                default:
                    throw new NotSupportedException($"Member access '{ma.Member.MemberType}' not supported");
            }
        }
    }

    class CallExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.Call;
        }

        public object GetValue(Expression expression)
        {
            var call = ((MethodCallExpression)expression);

            var targetObject = call.Object != null
                ? ViewModelExpressionValueProvidingTools.GetValue(call.Object)
                : null;

            var parameters = call.Method.GetParameters();

            var args = call.Arguments
                .Select((expr, i) =>
                {
                    var paramIType = parameters[i].ParameterType;

                    if (typeof(Expression).IsAssignableFrom(paramIType))
                    {
                        switch (expr.NodeType)
                        {
                            case ExpressionType.Lambda:
                                return expr;
                            case ExpressionType.Quote:
                                return ((UnaryExpression) expr).Operand;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    return ViewModelExpressionValueProvidingTools.GetValue(expr);
                })
                .ToArray();

            return call.Method.Invoke(targetObject, args);
        }
    }

    class NewExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.New;
        }

        public object GetValue(Expression expression)
        {
            var ne = (NewExpression)expression;

            return typeof(ViewModel).IsAssignableFrom(ne.Type) 
                ? ViewModel.Create(ne.Type, ne.Arguments.Select(ViewModelExpressionValueProvidingTools.GetValue).ToArray())
                : ne.Constructor.Invoke(ne.Arguments.Select(ViewModelExpressionValueProvidingTools.GetValue).ToArray()); 
        }
    }

    class MemberInitExpressionValueProvider : IExpressionValueProvider
    {
        public bool Predicate(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberInit;
        }

        public object GetValue(Expression expression)
        {
            var mi = (MemberInitExpression)expression;

            var obj = ViewModelExpressionValueProvidingTools.GetValue(mi.NewExpression);
            foreach (var memberBinding in mi.Bindings)
            {
                switch (memberBinding.BindingType)
                {
                    case MemberBindingType.Assignment:
                    {
                        var ma = (MemberAssignment)memberBinding;
                        var memberValue = ViewModelExpressionValueProvidingTools.GetValue(ma.Expression);

                        switch (memberBinding.Member.MemberType)
                        {
                            case MemberTypes.Field:
                                ((FieldInfo)memberBinding.Member).SetValue(obj, memberValue);
                                break;
                            case MemberTypes.Property:
                                ((PropertyInfo)memberBinding.Member).SetValue(obj, memberValue);
                                break;
                            default:
                                throw new NotSupportedException($"Member type '{memberBinding.Member.MemberType}' not supported");
                        }
                    }
                    break;
                    //case MemberBindingType.MemberBinding:
                    //    break;
                    case MemberBindingType.ListBinding:
                    {
                        var lb = (MemberListBinding) memberBinding;

                        switch (lb.Member.MemberType)
                        {
                            case MemberTypes.Property:
                            {
                                var propMember = (PropertyInfo) lb.Member;
                                var propertyVal = propMember.GetValue(obj);
                                foreach (var lbInitializer in lb.Initializers)
                                {
                                    lbInitializer.AddMethod.Invoke(propertyVal,
                                        lbInitializer.Arguments
                                            .Select(ViewModelExpressionValueProvidingTools.GetValue)
                                            .ToArray());
                                }
                            } 
                            break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return obj;
        }
    }
}
