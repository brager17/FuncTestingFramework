using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Extensions;
using FuncTestingFramework;
using FuncTestingFramework.Extensions;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class Person
    {
        public int Year { get; set; }
        public string Name { get; set; }
    }

    public class UnitTest1
    {
        private generator.ConfigurationBuilder Builder;

        public UnitTest1() => Builder = new generator.ConfigurationBuilder();

        [Fact]
        public void BetweenTest()
        {
            Func<int, int> func = x => 1;
            var call = Expression.Call(Expression.Constant(func.Target),func.Method,Expression.Constant(1));
            var configuration = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Between(1, 10);

            var value = FSTests.FunctionTester.gen(configuration);
        }
    }
}