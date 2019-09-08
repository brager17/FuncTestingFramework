using FuncTestingFramework.ObjectExtensions;
using Kimedics;
using Xunit;

namespace FuncTestingFrameworkClient
{
    public class BooleanTests
    {
        [Fact]
        public void IgnoreTest()
        {
            var ignoreConfiguration = Configuration.Build<Person>()
                .For(x => x.IsAdult).Ignore();
            
            
            var result = FSTests.gen(ignoreConfiguration);

            Assert.Equal(false,result.IsAdult);
        }
        
        [Fact]
        public void UseValueTestFalse()
        {
            var ignoreConfiguration = Configuration.Build<Person>()
                .For(x => x.IsAdult).UseValue(false);
            
            var result = FSTests.gen(ignoreConfiguration);

            Assert.Equal(false,result.IsAdult);
        }  
        
        [Fact]
        public void UseValueTestTrue()
        {
            var ignoreConfiguration = Configuration.Build<Person>()
                .For(x => x.IsAdult).UseValue(true);
            
            var result = FSTests.gen(ignoreConfiguration);

            Assert.Equal(true,result.IsAdult);
        }
    }
}