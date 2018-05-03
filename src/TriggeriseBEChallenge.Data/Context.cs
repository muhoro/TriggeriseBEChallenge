using Microsoft.EntityFrameworkCore;
using System;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            :base(options)
        {

        }

        public DbSet<Product> Product { get; set; }
    }
}
