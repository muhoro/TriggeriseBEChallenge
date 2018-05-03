using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.BusinessProcess.PricingRules
{
    using RuleFunc = Expression<Func<OrderLine, Price>>;
    public class TwoForOnePromotionRule : IPricingRule
    {
        private int target = 2;

        public RuleFunc PriceRuleFunc
            => e => _func(e);

        public TwoForOnePromotionRule()
        {
        }

        private Price _func(OrderLine orderLine)
        {
            if (!IsApplicable(orderLine)) throw new InvalidOperationException();
            int quantity = orderLine.Quantity;
            int pairs = quantity / 2;
            int mod = quantity % 2;
            decimal newprice = decimal.Zero;
            newprice = pairs * (orderLine.Product.DefaultPrice) + (mod * orderLine.Product.DefaultPrice);
            return new Price { Value = newprice };
        }

        public bool IsApplicable(OrderLine orderLine) =>
            (orderLine.Quantity >= target);
    }
}
