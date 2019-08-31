//using System.Collections.Generic;
//using FuncTestingFramework;
//using FuncTestingFramework.Decimal;
//using FuncTestingFramework.ObjectExtensions;
//using Kimedics;
//using Xunit;
//
//namespace FuncTestingFrameworkClient
//{
//    public class DecimalTests : IDefaultMethodTester<decimal>, IMinMaxIntervalMethodTester<decimal>
//    {
//        private readonly generator.ConfigurationBuilder Builder;
//        public DecimalTests() => Builder = new generator.ConfigurationBuilder();
//
//
//        [Theory]
//        [MemberData(nameof(Decimal_100))]
//        public void UseValueTest(decimal value)
//        {
//            var ignoreConfiguration = Builder
//                .Build<Person>()
//                .For(x => x.VisualAcuity)
//                .UseValue(value);
//
//            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
//            Assert.Equal(default(decimal), result.VisualAcuity);
//        }
//
//        [Fact]
//        public void IgnoreTest()
//        {
//            var ignoreConfiguration = Builder
//                .Build<Person>()
//                .For(x => x.VisualAcuity)
//                .Ignore();
//
//            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
//            Assert.Equal(default(decimal), result.VisualAcuity);
//        }
//
//        public static IEnumerable<object[]> Decimal_100() => Generators.RandomDecimal00();
//
//        [Theory]
//        [MemberData(nameof(Decimal_100))]
//        public void MinTest(decimal minValue)
//        {
//            var ignoreConfiguration = Builder
//                .Build<Person>()
//                .For(x => x.VisualAcuity)
//                .Min(minValue);
//
//            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
//            Assert.True(result.VisualAcuity >= minValue);
//        }
//
//        [Theory]
//        [MemberData(nameof(Decimal_100))]
//        public void MaxTest(decimal maxValue)
//        {
//            var ignoreConfiguration = Builder
//                .Build<Person>()
//                .For(x => x.VisualAcuity)
//                .Max(maxValue);
//
//            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
//            Assert.True(result.VisualAcuity <= maxValue);
//        }
//
//        [Theory]
//        [MemberData(nameof(Decimal_100))]
//        public void IntervalTest(decimal minValue, decimal maxValue)
//        {
//            var ignoreConfiguration = Builder
//                .Build<Person>()
//                .For(x => x.VisualAcuity)
//                .Interval(minValue, maxValue);
//
//            var result = FSTests.FunctionTester.gen(ignoreConfiguration);
//            Assert.True(result.VisualAcuity <= maxValue);
//            Assert.True(result.VisualAcuity >= minValue);
//        }
//    }
//}