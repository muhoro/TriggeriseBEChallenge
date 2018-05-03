﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TriggeriseBEChallenge.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal DefaultPrice { get; set; }

    }
}
