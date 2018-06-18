using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PlogBot.Data.Migrations
{
    public partial class AddTimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Times",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DiscordUserId = table.Column<ulong>(nullable: false),
                    TimeZone = table.Column<string>(nullable: true),
                    HasDaylightSavings = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Times", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Times_DiscordUserId",
                table: "Times",
                column: "DiscordUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Times");
        }
    }
}
