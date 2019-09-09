using FuncTestingFramework.ObjectExtensions;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class NesterEntityTests
    {
        [Fact]
        public void Test()
        {
            var ignoreConfiguration = Configuration
                .Build<Person>()
                .For(x => x.NestedPerson.Address)
                .ForNested(x => x.For(xx => xx.City).Length(1).For(xx => xx.City2));

            var result = Configuration.gen(ignoreConfiguration);
        }
    }
}