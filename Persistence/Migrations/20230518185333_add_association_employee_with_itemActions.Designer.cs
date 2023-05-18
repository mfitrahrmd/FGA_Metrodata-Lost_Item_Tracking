﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Context;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230518185333_add_association_employee_with_itemActions")]
    partial class add_association_employee_with_itemActions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("employee_id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("EmployeeId")
                        .HasName("pk_accounts");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AccountRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_account_roles");

                    b.HasIndex("AccountId")
                        .HasDatabaseName("ix_account_roles_account_id");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_account_roles_role_id");

                    b.ToTable("account_roles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Action", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_actions");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_actions_name");

                    b.ToTable("actions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_departments");

                    b.ToTable("departments", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("birth_date");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("department_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("first_name");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("gender");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("Nik")
                        .IsRequired()
                        .HasColumnType("char(5)")
                        .HasColumnName("nik");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("phone_number");

                    b.HasKey("Id")
                        .HasName("pk_employees");

                    b.HasAlternateKey("Nik")
                        .HasName("ak_employees_nik");

                    b.HasIndex("DepartmentId")
                        .HasDatabaseName("ix_employees_department_id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_email");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_phone_number")
                        .HasFilter("[phone_number] IS NOT NULL");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text")
                        .HasColumnName("image_path");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_items");

                    b.ToTable("items", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ItemActions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<Guid>("ActionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("action_id");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("employee_id");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit")
                        .HasColumnName("is_approved");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("item_id");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2")
                        .HasColumnName("time");

                    b.HasKey("Id")
                        .HasName("pk_item_actions");

                    b.HasIndex("ActionId")
                        .HasDatabaseName("ix_item_actions_action_id");

                    b.HasIndex("EmployeeId")
                        .HasDatabaseName("ix_item_actions_employee_id");

                    b.HasIndex("ItemId", "ActionId")
                        .IsUnique()
                        .HasDatabaseName("ix_item_actions_item_id_action_id");

                    b.ToTable("item_actions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Domain.Entities.Account", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_accounts_employees_employee_id");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.AccountRoles", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_account_roles_accounts_account_id");

                    b.HasOne("Domain.Entities.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_account_roles_roles_role_id");

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_employees_departments_department_id");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Domain.Entities.ItemActions", b =>
                {
                    b.HasOne("Domain.Entities.Action", "Action")
                        .WithMany("ItemActions")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_item_actions_actions_action_id");

                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("ItemActions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_item_actions_employees_employee_id");

                    b.HasOne("Domain.Entities.Item", "Item")
                        .WithMany("ItemActions")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_item_actions_items_item_id");

                    b.Navigation("Action");

                    b.Navigation("Employee");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Navigation("AccountRoles");
                });

            modelBuilder.Entity("Domain.Entities.Action", b =>
                {
                    b.Navigation("ItemActions");
                });

            modelBuilder.Entity("Domain.Entities.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("ItemActions");
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.Navigation("ItemActions");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Navigation("AccountRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
