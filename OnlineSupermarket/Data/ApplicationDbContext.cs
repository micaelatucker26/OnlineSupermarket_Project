using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineSupermarket.Models;
using OnlineSupermarket.Models.Entities;

namespace OnlineSupermarket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Item> Item {get; set;}
        public virtual DbSet<Profile> Profile {get; set;}
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
                entity.Property(e => e.Type).IsUnicode(false);
                entity.Property(e => e.Price);
                entity.Property(e => e.AmountInStock)
                    .IsUnicode(false);
                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.Item);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasMany(p => p.Item);
                entity.HasMany(p => p.Profile);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });
        }
    }
}
