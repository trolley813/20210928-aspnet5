using _20210928.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _20210928.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Review>()
                .HasOne(r => r.Item)
                .WithMany(i => i.Reviews);
            builder.Entity<Item>()
                .Property(i => i.Price)
                .HasConversion<Double>();
            base.OnModelCreating(builder);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
