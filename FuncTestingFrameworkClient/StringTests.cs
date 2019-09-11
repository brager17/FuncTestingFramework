using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFuncTestingFramework;
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

            var obj = Configuration.gen(configuration);
            Assert.True(obj.Name.Length <= length);
        }


        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void MinLength(int length)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .MinLength(length);

            var obj = Configuration.gen(configuration);
            Assert.True(obj.Name.Length >= length);
        }

        [Theory]
        [MemberData(nameof(String_100))]
        public void UseValueTest(string value)
        {
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .UseValue(value);

            var obj = Configuration.gen(configuration);
            Assert.Equal(value, obj.Name);
        }

        [Fact]
        public void IgnoreTest()
        {
            var configuration = Configuration
                .Build<Person>().For(x => x.Name)
                .Ignore();

            var obj = Configuration.gen(configuration);
            Assert.Equal(default(string), obj.Name);
        }


        [Theory]
        [MemberData(nameof(GenerateRandomInt32Less10000))]
        public void LengthTest(int length)
        {
            var c1 = Configuration.Build<Person>()
                .For(x => x.Name)
                .Length(2);
            var r = Configuration.gen(c1);
            
            var configuration = Configuration.Build<Person>()
                .For(x => x.Name)
                .Length(length);

            var obj = Configuration.gen(configuration);
            Assert.True(obj.Name.Length == length);
        }


        public static IEnumerable<object[]> GenerateRandomInt32Less10000()
        {
            return Generators.GenerateRandomInt32Less10000()
                .Select(x => Convert.ToInt32(x[0]) % 1000)
                .Select(x => new object[] {x})
                .ToList();
        }

        public static IEnumerable<object[]> String_100 => RandomString100();
    }
}