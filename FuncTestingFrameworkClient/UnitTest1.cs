using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Extensions;
using FuncTestingFramework;
using FuncTestingFramework.Extensions;
using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;
using Xunit.Abstractions;

namespace FuncTestingFrameworkClient
{
    public class NestedPerson
    {
        public int Year { get; set; }
        public string Name { get; set; }
    }

    public class Person
    {
        [Range(1, 100)]
        public int Year { get; set; }

        public string Name { get; set; }
        public NestedPerson NestedPerson { get; set; }

//        public bool IsAdult { get; set; }
        public decimal VisualAcuity { get; set; }
    }

    public class UnitTest1
    {
        private generator.ConfigurationBuilder Builder;
        private ITestOutputHelper ITestOutputHelper;

        public UnitTest1(ITestOutputHelper ITestOutputHelper)
        {
            Builder = new generator.ConfigurationBuilder();
            this.ITestOutputHelper = ITestOutputHelper;
        }

        [Fact]
        public void BetweenTest()
        {
            var s = int.MinValue;
            var t = s - 1;
        }
    }
}