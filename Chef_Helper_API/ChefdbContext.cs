using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Chef_Helper_API.Models;

namespace Chef_Helper_API;

public partial class ChefdbContext : DbContext
{
    public ChefdbContext()
    {
        Database.EnsureCreated();
    }

    public ChefdbContext(DbContextOptions<ChefdbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Recipes> Recipes { get; set; }

    public virtual DbSet<Warehouse> Warehouse { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-PV4HL9E\\MSSQLSERVER01;Initial Catalog=CHEFDB;Integrated Security=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipes>(entity =>
        {
            entity.HasKey(e => e.RecipeName).HasName("PK__RECIPES__9EFE16E84CFA31ED");

            entity.ToTable("RECIPES");

            entity.Property(e => e.RecipeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CalorieValue)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DishWeight)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.IngredientsNeeded)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.BoxNumber).HasName("PK__WAREHOUS__95A0B25921301404");

            entity.ToTable("WAREHOUSE");

            entity.Property(e => e.IngredientName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
