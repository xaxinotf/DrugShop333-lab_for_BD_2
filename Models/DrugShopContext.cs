﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DrugShop333.Models;

public partial class DrugShopContext : DbContext
{
    public DrugShopContext()
    {
    }

    public DrugShopContext(DbContextOptions<DrugShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShipType> ShipTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB; Database=DrugShop; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.ShipType, "IX_Orders_DeliveryId");

            entity.Property(e => e.DeliveryAddress).HasMaxLength(50);

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PaymentType");

            entity.HasOne(d => d.ShipTypeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_ShipType");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.ToTable("PaymentType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Desription).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(d => d.OrdersOrders).WithMany(p => p.NameProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProduct",
                    r => r.HasOne<Order>().WithMany().HasForeignKey("OrdersOrderId"),
                    l => l.HasOne<Product>().WithMany().HasForeignKey("NameProductId"),
                    j =>
                    {
                        j.HasKey("NameProductId", "OrdersOrderId");
                        j.ToTable("OrderProduct");
                        j.HasIndex(new[] { "OrdersOrderId" }, "IX_OrderProduct_OrdersOrderId");
                    });
        });

        modelBuilder.Entity<ShipType>(entity =>
        {
            entity.ToTable("ShipType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
