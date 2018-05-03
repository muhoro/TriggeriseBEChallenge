using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TriggeriseBEChallenge.BusinessProcess;
using TriggeriseBEChallenge.BusinessProcess.PricingRules;
using TriggeriseBEChallenge.ConsoleApp;
using TriggeriseBEChallenge.Data;
using TriggeriseBEChallenge.Domain;
using Xunit;

namespace TriggeriseBEChallenge.UnitTest
{
    public class CheckoutProcessTest
    {
        [Theory]
        [MemberData(nameof(Data), MemberType = typeof(CheckoutProcessTestData))]
        public async Task CheckoutProcessReturns_Correct_Total(List<string> productCodes, decimal actualPrice)
        {
            // Arrange
            var context = new Context(new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("stage").Options);
            context.AddRange(CheckoutProcessTestData.Products);
            context.SaveChanges();
            var repository = new Repository<Product>(context);

            var productRuleBinder = new ProductRuleBinder();
            productRuleBinder.Add(new Product { Code = "TICKET", DefaultPrice = 5.00M, Name = "Triggerise Ticket"}, new TwoForOnePromotionRule());
            productRuleBinder.Add(new Product { Code = "HOODIE", DefaultPrice = 5.00M, Name = "Triggerise Hoodie" }, new BulkPurchasePriceRule(3, 19.00M));

            // Act
            var checkoutProcess = new Checkout(repository, productRuleBinder);

            foreach(var code in productCodes)
                await checkoutProcess.Scan(code);

            var total = checkoutProcess.GetTotal();

            // Assert
            Assert.Equal(actualPrice, total);
        }
    }
}
