using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlogBot.Data.Migrations
{
    public partial class AddDiscordIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "DiscordId",
                table: "Plogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Recorded",
                table: "Logs",
                column: "Recorded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Recorded",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "Plogs");
        }
    }
}
