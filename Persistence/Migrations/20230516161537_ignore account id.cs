using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ignoreaccountid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_account_roles_accounts_account_temp_id",
                table: "account_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_account_roles_accounts_account_id",
                table: "account_roles",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_account_roles_accounts_account_id",
                table: "account_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_account_roles_accounts_account_temp_id",
                table: "account_roles",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
