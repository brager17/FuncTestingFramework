using System.Collections.Generic;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class DecimalTests : IDefaultMethodTester<decimal>, IMinMaxIntervalMethodTester<decimal>
    {
        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void UseValueTest(decimal value)
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .UseValue(value);

            var result = FSTests.gen(ignoreConfiguration);
            Assert.Equal(value, result.VisualAcuity);
        }

        [Fact]
        public void IgnoreTest()
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Ignore();


            var result = FSTests.gen(ignoreConfiguration);
            Assert.Equal(default(decimal), result.VisualAcuity);
        }

        public static IEnumerable<object[]> Decimal_100() => Generators.RandomDecimal00();

        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void MinTest(decimal minValue)
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Min(minValue);


            var result = FSTests.gen(ignoreConfiguration);
            Assert.True(result.VisualAcuity >= minValue);
        }

        [Theory]
        [MemberData(nameof(Decimal_100))]
        public void MaxTest(decimal maxValue)
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Max(maxValue);


            var result = FSTests.gen(ignoreConfiguration);
            Assert.True(result.VisualAcuity <= maxValue);
        }

        [Theory]
        [MemberData(nameof(IntervalTestData))]
        public void IntervalTest(decimal minValue, decimal maxValue)
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.VisualAcuity)
                .Interval(minValue, maxValue);


            var result = FSTests.gen(ignoreConfiguration);
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