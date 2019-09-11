using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CSharpFuncTestingFramework
{
    public static class RandomGenerator
    {
        public static Random rnd = new Random();
        public static int GenerateInt(int left = int.MinValue, int right = int.MaxValue) => rnd.Next(left, right);

        public static char GenerateChar() =>
            (rnd.Next() % 2 == 0 ? rnd.Next(65, 90) : rnd.Next(97, 122)).PipeTo(Convert.ToChar);

        public static string GenerateString(int left = int.MinValue, int right = int.MaxValue) =>
            Enumerable.Repeat(0, rnd.Next(left, right))
                .Select(_ => GenerateChar())
                .PipeTo(x => new string(x.ToArray()));
    }

    public static class RandomGeneratorMeta
    {
        private static readonly MethodInfo RandomGeneratorGenerateInt =
            typeof(RandomGenerator).GetMethod(nameof(RandomGenerator.GenerateInt));

        private static readonly MethodInfo RandomGeneratorGenerateString =
            typeof(RandomGenerator).GetMethod(nameof(RandomGenerator.GenerateString));

        public static MethodCallExpression GenerateIntCall(int left, int right) =>
            Expression.Call(RandomGeneratorGenerateInt, new object[] {left, right}.Select(Expression.Constant));

        public static MethodCallExpression GenerateStringCall(int left, int right) =>
            Expression.Call(RandomGeneratorGenerateString, new object[] {left, right}.Select(Expression.Constant));
    }
}