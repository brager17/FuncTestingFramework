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
            var ignoreConfiguration = Builder
                .Build<Person>()
                .For(x => x.SalaryByMonth)
                .Pipe(x => x
                    .Interval1(new Person(), new Person())
                    .Interval11(null, null));

        }
    }
}