using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Extensions;
using FuncTestingFramework;
using FuncTestingFramework.Extensions;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestingFrameworkClient
{
    public class NestedPerson
    {
        public int Year { get; set; }
        public string Name { get; set; }
    }

    public class Person
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public NestedPerson NestedPerson { get; set; }

//        public bool IsAdult { get; set; }
//        public decimal VisualAcuity { get; set; }
    }

    public class UnitTest1
    {
        private generator.ConfigurationBuilder Builder;
        private ITestOutputHelper ITestOutputHelper;

        public UnitTest1(ITestOutputHelper ITestOutputHelper)
        {
            Builder = new generator.ConfigurationBuilder();
            this.ITestOutputHelper = ITestOutputHelper;
        }

        [Fact]
        public void BetweenTest()
        {
            char s = (char)128;
            Console.WriteLine(s);
            Func<int, int> func = x => 1;
            var call = Expression.Call(Expression.Constant(func.Target), func.Method, Expression.Constant(1));
            var configuration = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Between(1, 10);

            var value = FSTests.FunctionTester.gen(configuration);
        }
    }
}