using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FuncTestingFramework;
using FuncTestingFramework.Decimal;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestingFrameworkClient
{
    public class DecimalTests : IDefaultMethodTester<decimal>, IMinMaxIntervalMethodTester<decimal>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly generator.ConfigurationBuilder Builder;

        public DecimalTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            Builder = new generator.ConfigurationBuilder();
        }

        private static ConcurrentQueue<decimal> values = new ConcurrentQueue<decimal>();

        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void UseValueTest(decimal value)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .UseValue(value);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
            Assert.Equal(value, result.VisualAcuity);
        }

        [Fact]
        public void IgnoreTest()
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Ignore();

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
            Assert.Equal(default(decimal), result.VisualAcuity);
        }

        public static IEnumerable<object[]> Decimal_100() => Generators.RandomDecimal00();

        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void MinTest(decimal minValue)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Min(minValue);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
            values.Enqueue(result.VisualAcuity);
            Assert.True(result.VisualAcuity >= minValue);
        }

        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void MaxTest(decimal maxValue)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Max(maxValue);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
            Assert.True(result.VisualAcuity <= maxValue);
        }

        [Theory]
        [MemberData(nameof(IntervalTestData))]
        public void IntervalTest(decimal minValue, decimal maxValue)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Interval(minValue, maxValue);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
            Assert.True(result.VisualAcuity <= maxValue);
            Assert.True(result.VisualAcuity >= minValue);
        }

        public static IEnumerable<object[]> IntervalTestData()
        {
            for (int i = 0; i < 100; i++)
            {
                var n1 = Generators.GetRandomDecimal();
                var n2 = Generators.GetRandomDecimal();
                yield return n1 < n2
                    ? new object[] {n1, n2}
                    : new object[] {n2, n1};
            }
        }
    }
}