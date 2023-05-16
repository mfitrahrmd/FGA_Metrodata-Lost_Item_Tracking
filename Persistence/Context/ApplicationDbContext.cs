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
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Employee>()
            .Property(e => e.Nik)
            .HasColumnType("char(5)");
        modelBuilder.Entity<Employee>()
            .Property(e => e.FirstName)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Employee>()
            .Property(e => e.LastName)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Employee>()
            .Property(e => e.Email)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Employee>()
            .Property(e => e.PhoneNumber)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.PhoneNumber)
            .IsUnique();
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Employee>()
            .HasAlternateKey(e => e.Nik);
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Items)
            .WithOne(i => i.Employee)
            .HasForeignKey(i => i.EmployeeId);
        modelBuilder.Entity<Employee>()
            .HasOne<Department>(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);

        modelBuilder.Entity<Item>()
            .Property(i => i.Name)
            .HasColumnType("varchar(100)");
        modelBuilder.Entity<Item>()
            .Property(i => i.ImagePath)
            .HasColumnType("text");
        modelBuilder.Entity<Item>()
            .Property(i => i.Description)
            .HasColumnType("text");
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
            .Property(a => a.Name)
            .HasColumnType("varchar(20)");
        modelBuilder.Entity<Action>()
            .HasIndex(a => a.Name)
            .IsUnique();
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

        modelBuilder.Entity<Department>()
            .Property(d => d.Name)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Department>()
            .HasMany<Employee>(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId);

        base.OnModelCreating(modelBuilder);
    }
}