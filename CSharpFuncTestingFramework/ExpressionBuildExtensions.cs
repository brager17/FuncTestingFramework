using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CSharpFuncTestingFramework
{
    public static class ExpressionBuildExtensions
    {
        public static Expression<Func<T, T>> MemberInit<T, T1>(Expression<Func<T, T1>> path, T1 v)
        {
            return MemberInit(path, Expression.Constant(v,typeof(T1)));
        }

        private static Expression<Func<T, T>> MemberInit<T, T1>(Expression<Func<T, T1>> path, Expression v)
        {
            var parameter = path.Parameters.Single();
            var assign = Expression.Assign(path.Body, v);
            var returnLabel = Expression.Label(typeof(T));
            var block = Expression.Block(assign, Expression.Label(returnLabel, parameter));
            var lambda = Expression.Lambda<Func<T, T>>(block, parameter);
            return lambda;
        }


        public static Expression<Func<T, T>> MemberInit<T, T1>(Expression<Func<T, T1>> path,
            MethodCallExpression callExpression) => MemberInit(path, (Expression) callExpression);
    }

    public static class Extensions
    {
        public static IEnumerable<T> Add<T>(this T v, IEnumerable<T> list) => list.Concat(new[] {v});
        public static IEnumerable<T> OneToList<T>(this T v) => new List<T>().Concat(new[] {v}).ToList();
        public static T1 PipeTo<T, T1>(this T v, Func<T, T1> f) => f(v);
    }
}