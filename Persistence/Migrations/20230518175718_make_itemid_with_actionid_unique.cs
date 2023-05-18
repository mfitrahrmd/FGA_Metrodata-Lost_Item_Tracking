﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class make_itemid_with_actionid_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_item_actions_item_id",
                table: "item_actions");

            migrationBuilder.CreateIndex(
                name: "ix_item_actions_item_id_action_id",
                table: "item_actions",
                columns: new[] { "item_id", "action_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_item_actions_item_id_action_id",
                table: "item_actions");

            migrationBuilder.CreateIndex(
                name: "ix_item_actions_item_id",
                table: "item_actions",
                column: "item_id");
        }
    }
}