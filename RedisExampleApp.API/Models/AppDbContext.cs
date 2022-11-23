﻿using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem 1", Price = 100m },
                new Product() { Id = 2, Name = "Kalem 2", Price = 200m },
                new Product() { Id = 3, Name = "Kalem 3", Price = 300m }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
