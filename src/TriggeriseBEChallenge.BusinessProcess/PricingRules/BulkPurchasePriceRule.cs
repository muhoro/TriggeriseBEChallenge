using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.BusinessProcess.PricingRules
{
    using RuleFunc = Expression<Func<OrderLine, Price>>;
    public class BulkPurchasePriceRule : IPricingRule
    {
        private int _target;
        private decimal _price;

        public BulkPurchasePriceRule(int quantityTarget, decimal price)
        {
            _target = quantityTarget;
            _price = price;
        }

        public RuleFunc PriceRuleFunc
            => e => _func(e);

        private Price _func(OrderLine orderLine)
        {
            if (!IsApplicable(orderLine)) throw new InvalidOperationException("Not Applicable");
            return new Price { Value = orderLine.Quantity * _price };
        }

        public bool IsApplicable(OrderLine orderLine)
        {
            if (orderLine == null) return false;
            return (orderLine.Quantity >= _target);
        }
    }
}
