using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestingFrameworkClient
{
    public class ArrayTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ArrayTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private Random Random = new Random();

        public int r() => Random.Next(1, 1000);

        [Theory]
        [MemberData(nameof(IntIntervalData))]
        public void IntervalTest(int min, int max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.SalaryByMonth)
                .ForItem(x => x.Between(min, max));

            var person = FSTests.gen(configuration);
            Assert.All(person.SalaryByMonth, salary => { Assert.True(salary > min && salary < max); });
        }

        [Theory]
        [MemberData(nameof(IntIntervalData))]
        public void Maxtest(int _, int max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.SalaryByMonth)
                .ForItem(x => x.Max(max));

            var person = FSTests.gen(configuration);
            Assert.All(person.SalaryByMonth, salary => { Assert.True(salary < max); });
        }

        [Theory]
        [MemberData(nameof(IntIntervalData))]
        public void MinTest(int min, int _)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.SalaryByMonth)
                .ForItem(x => x.Min(min));

            var person = FSTests.gen(configuration);
            Assert.All(person.SalaryByMonth, salary => { Assert.True(salary > min); });
        }

        [Theory]
        [MemberData(nameof(DateTimeIntervalData))]
        public void DateTimeInterval(DateTime min, DateTime max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Dates)
                .ForItem(x => x.Interval(min, max));

            var person = FSTests.gen(configuration);
            Assert.All(person.Dates, date => { Assert.True(date > min && date < max); });
        }

        [Theory]
        [MemberData(nameof(DateTimeIntervalData))]
        public void DateTimeMin(DateTime min, DateTime _)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Dates)
                .ForItem(x => x.Min(min));

            var person = FSTests.gen(configuration);
            Assert.All(person.Dates, date => { Assert.True(date > min); });
        }

        [Theory]
        [MemberData(nameof(DateTimeIntervalData))]
        public void DateTimeMax(DateTime _, DateTime max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Dates)
                .ForItem(x => x.Max(max));

            var person = FSTests.gen(configuration);
            Assert.All(person.Dates, date => { Assert.True(date < max); });
        }

        [Theory]
        [MemberData(nameof(DecimalIntervalData))]
        public void DecimalMax(decimal _, decimal max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Decimals)
                .ForItem(x => x.Max(max));

            var person = FSTests.gen(configuration);
            Assert.All(person.Decimals, dec => { Assert.True(dec < max); });
        }

        [Theory]
        [MemberData(nameof(DecimalIntervalData))]
        public void DecimalMin(decimal min, decimal _)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Decimals)
                .ForItem(x => x.Min(min));

            var person = FSTests.gen(configuration);
            Assert.All(person.Decimals, dec => { Assert.True(dec > min); });
        }

        [Theory]
        [MemberData(nameof(DecimalIntervalData))]
        public void DecimalInterval(decimal min, decimal max)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Decimals)
                .ForItem(x => x.Interval(min, max));

            var person = FSTests.gen(configuration);
            Assert.All(person.Decimals, dec => { Assert.True(dec > min && dec < max); });
        }


        public static IEnumerable<object[]> IntIntervalData()
        {
            for (int i = 0; i < 100; i++)
            {
                var n = Generators.GetRandomIntValue();
                var n1 = Generators.GetRandomIntValue();
                if (n < n1)
                    yield return new object[] {n, n1};
                else
                    yield return new object[] {n1, n};
            }
        }

        public static IEnumerable<object[]> DateTimeIntervalData()
        {
            for (int i = 0; i < 100; i++)
            {
                var n = Generators.GetRandomDateTime();
                var n1 = Generators.GetRandomDateTime();
                if (n < n1)
                    yield return new object[] {n, n1};
                else
                    yield return new object[] {n1, n};
            }
        }

        public static IEnumerable<object[]> DecimalIntervalData() => DecimalTests.IntervalTestData();
        
    }
}