using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;

namespace FuncTestingFrameworkClient
{
    public static class Generators
    {
        public static IEnumerable<object[]> RandomTuple2_100()
        {
            for (int i = 0; i < 100; i++)
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

        public static IEnumerable<object[]> RandomInt100()
        {
            for (int i = 0; i < 100; i++)
            {
                yield return new object[] {GetRandomIntValue()};
            }
        }

        public static IEnumerable<object[]> RandomString100()
        {
            return Enumerable.Range(1, 100).Select(_ => new object[] {Guid.NewGuid().ToString()});
        }

        public static IEnumerable<object[]> RandomDecimal00()
        {
            return Enumerable.Range(1, 100)
                .Select(_ => new object[]
                    {(decimal) rnd.Next(int.MinValue, int.MaxValue) * (decimal) rnd.NextDouble()});
        }

        public static IEnumerable<object[]> RandomBoolean100()
        {
            for (int i = 0; i < 100; i++)
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

        public static decimal GetRandomDecimal() => new Fixture().Create<decimal>();
        public static DateTime GetRandomDateTime() => new Fixture().Create<DateTime>();
        
    }
}