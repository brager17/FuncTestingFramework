using System;
using System.Collections.Generic;
using System.Linq;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;
using static FuncTestingFrameworkClient.Generators;

namespace FuncTestingFrameworkClient
{
    public class StringTests
    {
        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void MaxLength(int length)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .MaxLength(length);

            var obj = FSTests.gen(configuration);
            Assert.True(obj.Name.Length <= length);
        }


        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void MinLength(int length)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .MinLength(length);

            var obj = FSTests.gen(configuration);
            Assert.True(obj.Name.Length >= length);
        }

        [Theory]
        [MemberData(nameof(String_100))]
        public void UseValueTest(string value)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .UseValue(value);

            var obj = FSTests.gen(configuration);
            Assert.Equal(value, obj.Name);
        }

        [Fact]
        public void IgnoreTest()
        {
            var configuration = Configuration
                .Build<Person>().For(x => x.Name)
                .Ignore();

            var obj = FSTests.gen(configuration);
            Assert.Equal(default(string), obj.Name);
        }


        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void LengthTest(int length)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .Length(length);

            var obj = FSTests.gen(configuration);
            Assert.True(obj.Name.Length == length);
        }


        public static IEnumerable<object[]> GenerateRandomInt32Less10000()
        {
            var random = new Random();
            return Enumerable.Range(1, 100).Select(x => new object[] {random.Next(1, 1000)});
        }

        public static IEnumerable<object[]> String_100 => RandomString100();
    }
}