﻿using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Configurations;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Utils;

namespace ORM_Mini_Project.Contexts;

public class AppDbContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Constans.connectionString);
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}
