//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using FuncTestingFramework;
//using FuncTestingFramework.ObjectExtensions;
//using Xunit;
//
//namespace FuncTestingFrameworkClient
//{
//    public class NesterEntityTests
//    {
//        public object SetEnumerable<T>(object obj, IEnumerable<T> value, PropertyInfo propertyInfo)
//        {
//            propertyInfo.SetValue(obj, value);
//            return obj;
//        }
//
//        [Fact]
//        public void Test()
//        {
//            var ignoreConfigurationt = Configuration.Build<Person>()
//                .For(x => x.Persons)
//                .ForItem(x => x.For(xx => xx.Address.City).UseValue("123"));
//
//            var result1 = Configuration.gen(ignoreConfigurationt);
//
//            Assert.All(result1.Persons, person => Assert.Equal("123", person.Address.City));
//        }
//    }
//}