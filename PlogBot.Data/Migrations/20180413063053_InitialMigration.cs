using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlogBot.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Class = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    MainId = table.Column<Guid>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plogs_Plogs_MainId",
                        column: x => x.MainId,
                        principalTable: "Plogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Accuracy = table.Column<int>(nullable: false),
                    AdditionalDamage = table.Column<int>(nullable: false),
                    AttackPower = table.Column<int>(nullable: false),
                    Block = table.Column<int>(nullable: false),
                    BossAttackPower = table.Column<int>(nullable: false),
                    BossDefense = table.Column<int>(nullable: false),
                    ClanMemberId = table.Column<Guid>(nullable: false),
                    Concentration = table.Column<int>(nullable: false),
                    Critical = table.Column<int>(nullable: false),
                    CriticalDamage = table.Column<int>(nullable: false),
                    CriticalDefense = table.Column<int>(nullable: false),
                    DamageReduction = table.Column<int>(nullable: false),
                    DebuffDamage = table.Column<int>(nullable: false),
                    DebuffDefense = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    EarthDamage = table.Column<int>(nullable: false),
                    Evasion = table.Column<int>(nullable: false),
                    FlameDamage = table.Column<int>(nullable: false),
                    FrostDamage = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    HealthRegen = table.Column<int>(nullable: false),
                    HealthRegenCombat = table.Column<int>(nullable: false),
                    HongmoonLevel = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    LightningDamage = table.Column<int>(nullable: false),
                    Piercing = table.Column<int>(nullable: false),
                    PvpAttackPower = table.Column<int>(nullable: false),
                    PvpDefense = table.Column<int>(nullable: false),
                    Recorded = table.Column<DateTime>(nullable: false),
                    ShadowDamage = table.Column<int>(nullable: false),
                    WindDamage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Plogs_ClanMemberId",
                        column: x => x.ClanMemberId,
                        principalTable: "Plogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ClanMemberId",
                table: "Logs",
                column: "ClanMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Plogs_MainId",
                table: "Plogs",
                column: "MainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Plogs");
        }
    }
}
