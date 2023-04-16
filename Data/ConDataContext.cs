using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using BikeStores.Models.ConData;

namespace BikeStores.Data
{
    public partial class ConDataContext : DbContext
    {
        public ConDataContext()
        {
        }

        public ConDataContext(DbContextOptions<ConDataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BikeStores.Models.ConData.Stock>().HasKey(table => new {
                table.store_id, table.product_id
            });

            builder.Entity<BikeStores.Models.ConData.OrderItem>().HasKey(table => new {
                table.order_id, table.item_id
            });

            builder.Entity<BikeStores.Models.ConData.Product>()
              .HasOne(i => i.Brand)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.brand_id)
              .HasPrincipalKey(i => i.brand_id);

            builder.Entity<BikeStores.Models.ConData.Product>()
              .HasOne(i => i.Category)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.category_id)
              .HasPrincipalKey(i => i.category_id);

            builder.Entity<BikeStores.Models.ConData.Stock>()
              .HasOne(i => i.Product)
              .WithMany(i => i.Stocks)
              .HasForeignKey(i => i.product_id)
              .HasPrincipalKey(i => i.product_id);

            builder.Entity<BikeStores.Models.ConData.Stock>()
              .HasOne(i => i.Store)
              .WithMany(i => i.Stocks)
              .HasForeignKey(i => i.store_id)
              .HasPrincipalKey(i => i.store_id);

            builder.Entity<BikeStores.Models.ConData.OrderItem>()
              .HasOne(i => i.Order)
              .WithMany(i => i.OrderItems)
              .HasForeignKey(i => i.order_id)
              .HasPrincipalKey(i => i.order_id);

            builder.Entity<BikeStores.Models.ConData.OrderItem>()
              .HasOne(i => i.Product)
              .WithMany(i => i.OrderItems)
              .HasForeignKey(i => i.product_id)
              .HasPrincipalKey(i => i.product_id);

            builder.Entity<BikeStores.Models.ConData.Order>()
              .HasOne(i => i.Customer)
              .WithMany(i => i.Orders)
              .HasForeignKey(i => i.customer_id)
              .HasPrincipalKey(i => i.customer_id);

            builder.Entity<BikeStores.Models.ConData.Order>()
              .HasOne(i => i.Staff)
              .WithMany(i => i.Orders)
              .HasForeignKey(i => i.staff_id)
              .HasPrincipalKey(i => i.staff_id);

            builder.Entity<BikeStores.Models.ConData.Order>()
              .HasOne(i => i.Store)
              .WithMany(i => i.Orders)
              .HasForeignKey(i => i.store_id)
              .HasPrincipalKey(i => i.store_id);

            builder.Entity<BikeStores.Models.ConData.Staff>()
              .HasOne(i => i.Staff1)
              .WithMany(i => i.Staff2)
              .HasForeignKey(i => i.manager_id)
              .HasPrincipalKey(i => i.staff_id);

            builder.Entity<BikeStores.Models.ConData.Staff>()
              .HasOne(i => i.Store)
              .WithMany(i => i.Staff)
              .HasForeignKey(i => i.store_id)
              .HasPrincipalKey(i => i.store_id);
            this.OnModelBuilding(builder);
        }

        public DbSet<BikeStores.Models.ConData.Brand> Brands { get; set; }

        public DbSet<BikeStores.Models.ConData.Category> Categories { get; set; }

        public DbSet<BikeStores.Models.ConData.Product> Products { get; set; }

        public DbSet<BikeStores.Models.ConData.Stock> Stocks { get; set; }

        public DbSet<BikeStores.Models.ConData.Customer> Customers { get; set; }

        public DbSet<BikeStores.Models.ConData.OrderItem> OrderItems { get; set; }

        public DbSet<BikeStores.Models.ConData.Order> Orders { get; set; }

        public DbSet<BikeStores.Models.ConData.Staff> Staff { get; set; }

        public DbSet<BikeStores.Models.ConData.Store> Stores { get; set; }
    }
}