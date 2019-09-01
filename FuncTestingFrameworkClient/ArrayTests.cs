using System;
using FuncTestingFramework;
using FuncTestingFramework.DateTimeExtensions;
using FuncTestingFramework.ObjectExtensions;
using FuncTestingFramework.Sequence;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class ArrayTests
    {
        private readonly generator.ConfigurationBuilder Builder;


        public void PipeTest()
        {
            IConfiguration<int> s = null;
            
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.SalaryByMonth);
        }
    }
}