using Newtonsoft.Json;
using Remote.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TriggeriseBEChallenge.Domain
{
    public class Order
    {
        private Order(IList<OrderLine> orderLines)
        {
            OrderLines = orderLines;
        }

        public static Order Create(IList<OrderLine> orderLines) 
            => new Order(orderLines);

        public IList<OrderLine> OrderLines { get; private set; }
        public DateTime OrderDate => DateTime.UtcNow;
        public decimal TotalPrice {
            get
            {
                decimal total = decimal.Zero;
                OrderLines.ToList().ForEach((o) => total += o.Price);
                return total;
            } }
    }
}
