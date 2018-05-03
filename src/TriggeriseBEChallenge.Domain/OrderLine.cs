using System;
using System.Collections.Generic;
using System.Text;

namespace TriggeriseBEChallenge.Domain
{
    public class OrderLine
    {
        public OrderLine(Guid id, Product product, int quantity, decimal price)
        {
            Id = id;
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public Guid Id { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

    }
}
