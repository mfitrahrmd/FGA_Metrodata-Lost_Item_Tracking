using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class remove_association_employe_with_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_employees_employee_id",
                table: "items");

            migrationBuilder.DropIndex(
                name: "ix_items_employee_id",
                table: "items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_items_employee_id",
                table: "items",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "fk_items_employees_employee_id",
                table: "items",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
