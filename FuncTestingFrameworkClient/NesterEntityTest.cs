//using FuncTestingFramework.ObjectExtensions;
//using Xunit;
//
//namespace FuncTestingFrameworkClient
//{
//    public class NesterEntityTests
//    {
//        [Fact]
//        public void Test()
//        {
//            var ignoreConfiguration = Configuration
//                .Build<Person>()
//                .For(x => x.NestedPerson.Address)
//                .ForNested<Address>(x=>x.)
//
//            var result = Configuration.gen(ignoreConfiguration);
//            Assert.Equal(value, result.VisualAcuity);
//        }
//        
//    }
//}