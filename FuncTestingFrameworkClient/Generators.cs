using System;
using System.Collections.Generic;

namespace FuncTestingFrameworkClient
{
    public static class Generators
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
    }
}