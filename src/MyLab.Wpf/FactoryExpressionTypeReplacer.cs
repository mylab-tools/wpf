using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyLab.Wpf
{
    static class FactoryExpressionTypeReplacer
    {
        public static Expression<Func<T>> Replace<T>(Expression<Func<T>> factoryFunc, Type newType)
        {
            if (factoryFunc == null) throw new ArgumentNullException(nameof(factoryFunc));

            switch (factoryFunc.Body.NodeType)
            {
                case ExpressionType.MemberInit:
                {
                    var initialMemberInitExpr = (MemberInitExpression)factoryFunc.Body;

                    var newExpr = Expression.New(GetNewCtor(initialMemberInitExpr.NewExpression.Constructor, newType), initialMemberInitExpr.NewExpression.Arguments);

                    var memberInitExpr = Expression.MemberInit(newExpr, initialMemberInitExpr.Bindings);

                    return Expression.Lambda<Func<T>>(memberInitExpr);

                }
                case ExpressionType.New:
                {
                    var initialNewExpr = (NewExpression) factoryFunc.Body;
                    var newExpr = Expression.New(GetNewCtor(initialNewExpr.Constructor, newType),
                        initialNewExpr.Arguments);

                    return Expression.Lambda<Func<T>>(newExpr);
                }
                default:
                    throw new NotSupportedException($"Expression {factoryFunc.NodeType} not supported");
            }
        }

        static ConstructorInfo GetNewCtor(ConstructorInfo oldTypeCtor, Type newType)
        {
            var parameterTypes= oldTypeCtor.GetParameters().Select(t => t.ParameterType).ToArray();
            var ctor = newType.GetConstructor(parameterTypes);

            if (ctor == null)
                throw new InvalidOperationException($"The type {newType.FullName} does not contains the same ctor as {oldTypeCtor.DeclaringType?.FullName}: {oldTypeCtor.ToString()}");

            return ctor;
        }
    }
}
