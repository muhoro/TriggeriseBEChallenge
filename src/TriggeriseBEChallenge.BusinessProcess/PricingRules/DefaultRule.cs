using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.BusinessProcess.PricingRules
{
    using RuleFunc = Expression<Func<OrderLine, Price>>;
    public class DefaultRule : IPricingRule
    {
        public RuleFunc PriceRuleFunc
            => e => _func(e);

        private Price _func(OrderLine orderLine)
            => new Price { Value = orderLine.Quantity * orderLine.Product.DefaultPrice };

        public bool IsApplicable(OrderLine orderLine)
            => orderLine != null;
    }
}
