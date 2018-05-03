using System;
using System.Collections.Generic;
using System.Text;
using TriggeriseBEChallenge.Data;
using TriggeriseBEChallenge.Domain;
using System.Linq;
using TriggeriseBEChallenge.BusinessProcess;
using TriggeriseBEChallenge.BusinessProcess.PricingRules;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TriggeriseBEChallenge.ConsoleApp
{
    public class Checkout
    {
        private List<Product> _products = new List<Product>();

        private readonly ProductRuleBinder _productRuleBinder;
        private readonly IRepository<Product> _repository;

        public Checkout(IRepository<Product> repository, ProductRuleBinder productRuleBinder)
        {
            _productRuleBinder = productRuleBinder ?? throw new ArgumentNullException(nameof(productRuleBinder));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Scan(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return;

            var product = _repository.Get(filter: c => c.Code.Equals(productCode, StringComparison.OrdinalIgnoreCase))
                .ToList().FirstOrDefault();

            if (product != null)
                _products.Add(product);
        }

        public decimal GetTotal()
        {
            List<OrderLine> orderLines = new List<OrderLine>();
            List<OrderLine> newOrderLines = new List<OrderLine>();

            orderLines.AddRange(_products.GroupBy(e => e.Code)
                .Select(d => new OrderLine(Guid.NewGuid(), d.First(), d.Count(), d.First().DefaultPrice)));

            foreach(var orderline in orderLines)
            {
                IPricingRule rule = new DefaultRule();
                var pricingRules = _productRuleBinder.GetRules(orderline.Product);
                foreach(var prule in pricingRules)
                    if (prule.IsApplicable(orderline))
                    {
                        rule = prule;
                        break;
                    }

                var price = rule.PriceRuleFunc.Compile().Invoke(orderline).Value;
                newOrderLines.Add(new OrderLine(orderline.Id, orderline.Product, orderline.Quantity, price));
            }

            var order = Order.Create(newOrderLines);
            return order.TotalPrice;
        }
    }
}
