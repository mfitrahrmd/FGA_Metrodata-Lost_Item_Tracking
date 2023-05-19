using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class edit_itemactions_only_have_exacly_one_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_status",
                table: "status");

            migrationBuilder.DropIndex(
                name: "ix_status_item_actions_id",
                table: "status");

            migrationBuilder.DropColumn(
                name: "id",
                table: "status");

            migrationBuilder.DropColumn(
                name: "is_approved",
                table: "item_actions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_status",
                table: "status",
                column: "item_actions_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_status",
                table: "status");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "status",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "is_approved",
                table: "item_actions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "pk_status",
                table: "status",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_status_item_actions_id",
                table: "status",
                column: "item_actions_id",
                unique: true);
        }
    }
}
