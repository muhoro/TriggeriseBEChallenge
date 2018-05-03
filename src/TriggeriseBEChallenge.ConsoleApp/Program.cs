using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Remote.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TriggeriseBEChallenge.BusinessProcess;
using TriggeriseBEChallenge.BusinessProcess.PricingRules;
using TriggeriseBEChallenge.Data;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Triggerise BE Challenge!");

            var services = new ServiceCollection();
            services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("stage"));

            var products = new List<Product>
            {
                new Product{ Id = Guid.NewGuid(), Code = "TICKET", Name = "Triggerise Ticket", DefaultPrice = 5.00M},
                new Product{ Id = Guid.NewGuid(), Code = "HOODIE", Name = "Triggerise Hoodie", DefaultPrice = 20.00M},
                new Product{ Id = Guid.NewGuid(), Code = "HAT", Name = "Triggerise Hat", DefaultPrice = 7.50M}
            };
            var productrepository = new Repository<Product>(services.BuildServiceProvider().GetService<Context>());
            productrepository.AddRangeAsync(products).Wait();
            productrepository.SaveChanges();

            services.AddScoped(typeof(ProductRuleBinder), s =>
            {
                var _productRuleBinder = new ProductRuleBinder();
                var ticketproduct = productrepository.Get(filter: x => x.Code.Equals("TICKET", StringComparison.OrdinalIgnoreCase)).First();
                var hoodieproduct = productrepository.Get(filter: x => x.Code.Equals("HOODIE", StringComparison.OrdinalIgnoreCase)).First();

                _productRuleBinder.Add(ticketproduct, new TwoForOnePromotionRule());
                _productRuleBinder.Add(hoodieproduct, new BulkPurchasePriceRule(3, 19.00M));

                return _productRuleBinder;
            });

            var productRuleBinder = services.BuildServiceProvider().GetService<ProductRuleBinder>();
            var checkout = new Checkout(productrepository, productRuleBinder);

            checkout.Scan("TICKET").Wait();
            checkout.Scan("HOODIE").Wait();
            checkout.Scan("HAT").Wait();

            var total = checkout.GetTotal();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total = $ {total}");
            Console.ReadKey();
        }
    }
}
