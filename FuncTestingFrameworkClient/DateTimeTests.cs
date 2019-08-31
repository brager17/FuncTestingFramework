using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using Extensions;
using FuncTestingFramework;
using FuncTestingFramework.DateTimeExtensions;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class DateTimeTests
    {
        private readonly generator.ConfigurationBuilder Builder;

        public DateTimeTests() => Builder = new generator.ConfigurationBuilder();

        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void UseValueTest(DateTime value)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.Birthday)
                .UseValue(value);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);

            Assert.Equal(value, result.Birthday);
        }

        [Fact]
        public void IgnoreTest()
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.Birthday)
                .Ignore();

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);

            Assert.Equal(default(DateTime), result.Birthday);
        }


        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void MinTest(DateTime min)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.Birthday)
                .Min(min);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);

            Assert.True(min < result.Birthday);
        }


        [Theory]
        [MemberData(nameof(RandomDateTime))]
        public void MaxTest(DateTime max)
        {
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.Birthday)
                .Max(max);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);

            Assert.True(max > result.Birthday);
        }


        [Theory]
        [MemberData(nameof(IntervalTestDate))]
        public void IntervalTest(DateTime value1, DateTime value)
        {
            var max = value1 > value ? value1 : value;
            var min = value1 < value ? value1 : value;
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.Birthday)
                .Interval(min, max);

            var result = FSTests.FunctionTester.gen(ignoreConfiguration);

            Assert.True(min < result.Birthday);
            Assert.True(max > result.Birthday);
        }

        public static IEnumerable<object[]> RandomDateTime() =>
            Enumerable.Repeat(1, 100).Select(_ => new object[] {Generators.GetRandomDateTime()});

        public static IEnumerable<object[]> IntervalTestDate()
        {
            var v1 = Generators.GetRandomDateTime();
            var v2 = Generators.GetRandomDateTime();
            for (var i = 0; i < 100; i++)
            {
                if (v1 < v2)
                    yield return new object[] {v1, v2};
                else
                    yield return new object[] {v2, v1};
            }
        }
    }
}