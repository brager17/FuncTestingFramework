using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuncTestingFrameworkClient
{
    public class NestedPerson
    {
        public int Year { get; set; }
        public string Name { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string City1 { get; set; }
        public string City2 { get; set; }
    }

    public class Person
    {
        [Range(1, 100)]
        public int Year { get; set; }

        public DateTime Birthday { get; set; }

        public string Name { get; set; }
        public NestedPerson NestedPerson { get; set; }

        public bool IsAdult { get; set; }
        public decimal VisualAcuity { get; set; }

        public IEnumerable<int> SalaryByMonth { get; set; }
        public IEnumerable<DateTime> Dates { get; set; }
        public IEnumerable<decimal> Decimals { get; set; }

        public IEnumerable<string> Names { get; set; }
//        public IEnumerable<NestedPerson> Persons { get; set; }
    }
}