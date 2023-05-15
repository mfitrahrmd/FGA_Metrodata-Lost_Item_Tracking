using System.Reflection;
using Application.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

namespace Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<ItemActions> ItemActions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Items)
            .WithOne(i => i.Employee)
            .HasForeignKey(i => i.EmployeeId);

        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Item>()
            .HasOne<Employee>(i => i.Employee)
            .WithMany(e => e.Items)
            .HasForeignKey(i => i.EmployeeId);
        modelBuilder.Entity<Item>()
            .HasMany<ItemActions>(i => i.ItemActions)
            .WithOne(ia => ia.Item)
            .HasForeignKey(ia => ia.ItemId);

        modelBuilder.Entity<Action>()
            .HasKey(a => a.Id);
        modelBuilder.Entity<Action>()
            .HasMany<ItemActions>(a => a.ItemActions)
            .WithOne(ia => ia.Action)
            .HasForeignKey(ia => ia.ActionId);

        modelBuilder.Entity<ItemActions>()
            .HasKey(ia => ia.Id);
        modelBuilder.Entity<ItemActions>()
            .HasOne<Item>(ia => ia.Item)
            .WithMany(i => i.ItemActions)
            .HasForeignKey(ia => ia.ItemId);
        modelBuilder.Entity<ItemActions>()
            .HasOne<Action>(ia => ia.Action)
            .WithMany(i => i.ItemActions)
            .HasForeignKey(ia => ia.ActionId);

        base.OnModelCreating(modelBuilder);
    }
}