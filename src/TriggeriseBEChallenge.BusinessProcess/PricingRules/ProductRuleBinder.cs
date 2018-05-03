using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TriggeriseBEChallenge.BusinessProcess.PricingRules;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.BusinessProcess
{
    public enum RulePriority
    {
        High = 1,
        Medium = 2,
        Low = 3
    }

    public class ProductRuleBinder
    {
        private IList<IPricingRule> rules;

        private IDictionary<string, List<IPricingRule>> _dict 
            = new Dictionary<string, List<IPricingRule>>();

        public void Add(Product product, IPricingRule rule, RulePriority rulePriority = RulePriority.High)
        {
            if (_dict.TryGetValue(product.Code.Trim(), out List<IPricingRule> rls))
            {
                rls.Add(rule);
                _dict.Remove(product.Code.Trim());
                _dict.Add(product.Code.Trim(), rls);
            }
            else
                _dict.Add(product.Code.Trim(), new List<IPricingRule> { rule });
        }

        public IEnumerable<IPricingRule> GetRules(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            if (_dict.TryGetValue(product.Code.Trim(), out List<IPricingRule> rls))
                return rls;
            return new List<IPricingRule>() { new DefaultRule() };
        }

        public IPricingRule GetRule(Product product)
        {
            if (_dict.TryGetValue(product.Code.Trim(), out List<IPricingRule> rls))
                return rls.First();

            return new DefaultRule();
        }
    }
}
