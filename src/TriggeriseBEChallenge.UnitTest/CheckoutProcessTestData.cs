using System;
using System.Collections.Generic;
using System.Text;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.UnitTest
{
    public class CheckoutProcessTestData
    {
        public static IEnumerable<object[]> Data =>

            new List<object[]>()
            {
                new object[] { new List<string> { "TICKET", "HOODIE", "HAT" },  32.50M},
                new object[] { new List<string> { "TICKET", "HOODIE", "TICKET" }, 25.00M },
                new object[] { new List<string> { "HOODIE", "HOODIE", "HOODIE", "TICKET", "HOODIE" }, 81.00M },
                new object[] { new List<string> { "TICKET", "HOODIE", "TICKET", "TICKET", "HAT", "HOODIE", "HOODIE" }, 74.50M }
            };

        public static List<Product> Products =>

            new List<Product>
            {
                new Product{ Id = Guid.NewGuid(), Code = "TICKET", Name = "Triggerise Ticket", DefaultPrice = 5.00M},
                new Product{ Id = Guid.NewGuid(), Code = "HOODIE", Name = "Triggerise Hoodie", DefaultPrice = 20.00M},
                new Product{ Id = Guid.NewGuid(), Code = "HAT", Name = "Triggerise Hat", DefaultPrice = 7.50M}
            };
    }
}
