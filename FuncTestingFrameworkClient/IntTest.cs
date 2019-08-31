using System.Collections.Generic;
using Extensions;
using FuncTestingFramework;
using Kimedics;
using Xunit;
using FuncTestingFramework.ObjectExtensions;
namespace FuncTestingFrameworkClient
{
    public class IntTest
    {
        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void MinTest(int @int)
        {
            var configurationMin = Builder
                .Build<Person>()
                .For(x => x.Year)
                .Min(@int);

            Assert.True(FSTests.FunctionTester.gen(configurationMin).Year > @int);
        }

        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void MaxTest(int @int)
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
        [MemberData(nameof(RandomInt100))]
        public void UseValueTest(int value)
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.Year)
                .UseValue(value);
            Assert.Equal(value, FSTests.FunctionTester.gen(configurationMax).Year);
        }

        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void NestedUseValueTest(int value)
        {
            var configurationMax = Builder
                .Build<Person>()
                .For(x => x.NestedPerson.Year)
                .UseValue(value);
            Assert.Equal(value, FSTests.FunctionTester.gen(configurationMax).NestedPerson.Year);
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

        public static IEnumerable<object[]> RandomInt100 => Generators.RandomInt100();
    }
}