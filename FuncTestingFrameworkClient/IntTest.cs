using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using FuncTestingFramework;
using FuncTestingFramework.Extensions;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class IntTest
    {
        public static IEnumerable<object[]> RandomTuple2_1000()
        {
            for (int i = 0; i < 1000; i++)
            {
                var l = 0;
                var r = 0;

                while (l >= r)
                {
                    l = GetRandomIntValue();
                    r = GetRandomIntValue();
                }

                yield return new object[] {l, r};
            }
        }

        public static IEnumerable<object[]> RandomInt1000()
        {
            for (int i = 0; i < 1000; i++)
            {
                yield return new object[] {GetRandomIntValue()};
            }
        }

        public static IEnumerable<object[]> RandomBoolean1000()
        {
            for (int i = 0; i < 1000; i++)
            {
                yield return new object[] {GetRandomIntValue() % 2 == 0};
            }
        }
        
        private static Random rnd = new Random();

        public static int GetRandomIntValue(int l = 0, int r = 0) =>
            l == 0 && r == 0 ? rnd.Next() :
            l == 0 && r != 0 ? rnd.Next(int.MinValue, r) :
            l != 0 && r == 0 ? rnd.Next(l, int.MaxValue) :
            rnd.Next();
        
        [Theory]
        [MemberData(nameof(RandomTuple2_1000))]
        public void BetweenTest(int left, int rigth)
        {
            var configuration = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Between(left, rigth);

            var value = FSTests.FunctionTester.gen(configuration);

            Assert.True(value.Year > left && value.Year < rigth);
        }

        [Theory]
        [MemberData(nameof(RandomInt1000))]
        public void MinTest(int @int)
        {
            var configurationMin = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Min(@int);

            Assert.True(FSTests.FunctionTester.gen(configurationMin).Year > @int);
        }

        [Theory]
        [MemberData(nameof(RandomInt1000))]
        public void Test(int @int)
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Max(@int);
            Assert.True(FSTests.FunctionTester.gen(configurationMax).Year < @int);
        }


        [Fact]
        public void PositiveTest()
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Positive();
            Assert.True(FSTests.FunctionTester.gen(configurationMax).Year > 0);
        }

        [Fact]
        public void NegativeTest()
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Negative();
            Assert.True(FSTests.FunctionTester.gen(configurationMax).Year < 0);
        }


        [Theory]
        [MemberData(nameof(RandomInt1000))]
        public void UseValueTest(int value)
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .UseValue(value);
            Assert.Equal(value, FSTests.FunctionTester.gen(configurationMax).Year);
        }

        [Fact]
        public void IgnoreTest()
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Ignore();
            Assert.Equal(0, FSTests.FunctionTester.gen(configurationMax).Year);
        }

        private readonly generator.ConfigurationBuilder Builder;
        public IntTest() => Builder = new generator.ConfigurationBuilder();

    }
}