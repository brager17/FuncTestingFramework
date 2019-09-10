using System.Collections.Generic;
using FuncTestingFramework;
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
            var configurationMin = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .Min(@int);

            Assert.True(Configuration.gen(configurationMin).Year > @int);
        }

        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void MaxTest(int @int)
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .Max(@int)
                .Min(123)
                .For(x => x.Name)
                .Ignore();

            Assert.True(Configuration.gen(configurationMax).Year < @int);
        }


        [Fact]
        public void PositiveTest()
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .Positive();

            Assert.True(Configuration.gen(configurationMax).Year > 0);
        }

        [Fact]
        public void NegativeTest()
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .Negative();

            Assert.True(Configuration.gen(configurationMax).Year < 0);
        }


        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void UseValueTest(int value)
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .UseValue(value);

            Assert.Equal(value, Configuration.gen(configurationMax).Year);
        }

        [Theory]
        [MemberData(nameof(RandomInt100))]
        public void NestedUseValueTest(int value)
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.NestedPerson.Year)
                .UseValue(value);

            Assert.Equal(value, Configuration.gen(configurationMax).NestedPerson.Year);
        }

        [Fact]
        public void IgnoreTest()
        {
            var configurationMax = Configuration
                .Build<Person>()
                .For(x => x.Year)
                .Ignore();

            Assert.Equal(0, Configuration.gen(configurationMax).Year);
        }


        public static IEnumerable<object[]> RandomInt100 => Generators.RandomInt100();
    }
}