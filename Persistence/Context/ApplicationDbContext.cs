#region

using System.Reflection;
using Application.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

#endregion

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
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<AccountRoles> AccountRoles { get; set; }
    public DbSet<Status> Status { get; set; }

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
            .HasOne<Department>(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
        modelBuilder.Entity<Employee>()
            .HasOne<Account>(e => e.Account)
            .WithOne(a => a.Employee)
            .HasForeignKey<Account>(a => a.EmployeeId);
        modelBuilder.Entity<Employee>()
            .HasMany<ItemActions>(e => e.ItemActions)
            .WithOne(ia => ia.Employee)
            .HasForeignKey(ia => ia.EmployeeId);

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
        modelBuilder.Entity<ItemActions>()
            .HasIndex(ia => new { ia.ItemId, ia.ActionId, ia.EmployeeId }).IsUnique();
        modelBuilder.Entity<ItemActions>()
            .HasOne<Employee>(ia => ia.Employee)
            .WithMany(e => e.ItemActions)
            .HasForeignKey(ia => ia.EmployeeId);
        modelBuilder.Entity<ItemActions>()
            .HasOne<Status>(ia => ia.Status)
            .WithOne(s => s.ItemActions)
            .HasForeignKey<Status>(s => s.ItemActionsId);

        modelBuilder.Entity<Department>()
            .HasKey(d => d.Id);
        modelBuilder.Entity<Department>()
            .Property(d => d.Name)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Department>()
            .HasMany<Employee>(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId);

        modelBuilder.Entity<Account>()
            .HasKey(a => a.EmployeeId);
        modelBuilder.Entity<Account>()
            .Property(a => a.Password)
            .HasColumnType("text");
        modelBuilder.Entity<Account>()
            .HasOne<Employee>(a => a.Employee)
            .WithOne(e => e.Account)
            .HasForeignKey<Account>(a => a.EmployeeId);
        modelBuilder.Entity<Account>()
            .HasMany<AccountRoles>(a => a.AccountRoles)
            .WithOne(ar => ar.Account)
            .HasForeignKey(ar => ar.AccountId);
        modelBuilder.Entity<Account>()
            .Ignore(a => a.Id);

        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Role>()
            .HasMany<AccountRoles>(r => r.AccountRoles)
            .WithOne(ar => ar.Role)
            .HasForeignKey(ar => ar.RoleId);

        modelBuilder.Entity<AccountRoles>()
            .HasKey(ar => ar.Id);
        modelBuilder.Entity<AccountRoles>()
            .HasOne<Account>(ar => ar.Account)
            .WithMany(a => a.AccountRoles)
            .HasForeignKey(ar => ar.AccountId);
        modelBuilder.Entity<AccountRoles>()
            .HasOne<Role>(ar => ar.Role)
            .WithMany(r => r.AccountRoles)
            .HasForeignKey(ar => ar.RoleId);

        modelBuilder.Entity<Status>()
            .HasKey(s => s.ItemActionsId);
        modelBuilder.Entity<Status>()
            .Property(s => s.Name)
            .HasColumnType("varchar(50)");
        modelBuilder.Entity<Status>()
            .Property(s => s.Message)
            .HasColumnType("text");
        modelBuilder.Entity<Status>()
            .HasOne<ItemActions>(s => s.ItemActions)
            .WithOne(ia => ia.Status)
            .HasForeignKey<Status>(s => s.ItemActionsId);
        modelBuilder.Entity<Status>()
            .Ignore(s => s.Id);
        
        
        base.OnModelCreating(modelBuilder);
    }
}