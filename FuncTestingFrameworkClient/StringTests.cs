using System;
using System.Collections.Generic;
using System.Linq;
using FuncTestingFramework;
using FuncTestingFramework.Extensions;
using Kimedics;
using Xunit;
using Xunit.Abstractions;
using static FuncTestingFrameworkClient.Generators;

namespace FuncTestingFrameworkClient
{
    public class StringTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly generator.ConfigurationBuilder Builder;

        public StringTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            Builder = new generator.ConfigurationBuilder();
        }

        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void MinLength(int length)
        {
            _outputHelper.WriteLine(Guid.NewGuid().ToString().Length.ToString());
            var configuration = Builder.Build<Person>()
                .For(x => x.Name)
                .MaxLength(length);

            var obj = FSTests.FunctionTester.gen(configuration);
            Assert.True(obj.Name.Length <= length);
        }

        public static IEnumerable<object[]> GenerateRandomInt32Less10000 =>
            Enumerable.Range(1, 10000).Select(x => new object[] {new Random().Next(1, 1000)});
    }
}