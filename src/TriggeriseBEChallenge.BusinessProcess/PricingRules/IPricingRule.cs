using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.BusinessProcess.PricingRules
{
    using RuleFunc = Expression<Func<OrderLine, Price>>;
    public interface IPricingRule
    {
        bool IsApplicable(OrderLine orderLine);
        RuleFunc PriceRuleFunc { get; }
    }
}
