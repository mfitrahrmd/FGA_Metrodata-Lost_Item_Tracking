using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class add_association_employee_with_itemActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "employee_id",
                table: "item_actions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_item_actions_employee_id",
                table: "item_actions",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "fk_item_actions_employees_employee_id",
                table: "item_actions",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_item_actions_employees_employee_id",
                table: "item_actions");

            migrationBuilder.DropIndex(
                name: "ix_item_actions_employee_id",
                table: "item_actions");

            migrationBuilder.DropColumn(
                name: "employee_id",
                table: "item_actions");
        }
    }
}
