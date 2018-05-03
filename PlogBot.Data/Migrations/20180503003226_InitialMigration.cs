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
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImgUrl = table.Column<string>(nullable: true),
                    ItemType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Class = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    DiscordId = table.Column<ulong>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
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
                    BatchId = table.Column<int>(nullable: false),
                    BeltId = table.Column<int>(nullable: true),
                    Block = table.Column<int>(nullable: false),
                    BossAttackPower = table.Column<int>(nullable: false),
                    BossDefense = table.Column<int>(nullable: false),
                    BraceletId = table.Column<int>(nullable: true),
                    ClanMemberId = table.Column<Guid>(nullable: false),
                    Concentration = table.Column<int>(nullable: false),
                    Critical = table.Column<int>(nullable: false),
                    CriticalDamage = table.Column<int>(nullable: false),
                    CriticalDefense = table.Column<int>(nullable: false),
                    DamageReduction = table.Column<int>(nullable: false),
                    DebuffDamage = table.Column<int>(nullable: false),
                    DebuffDefense = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    EarringId = table.Column<int>(nullable: true),
                    EarthDamage = table.Column<int>(nullable: false),
                    Evasion = table.Column<int>(nullable: false),
                    FlameDamage = table.Column<int>(nullable: false),
                    FrostDamage = table.Column<int>(nullable: false),
                    Gem1Id = table.Column<int>(nullable: true),
                    Gem2Id = table.Column<int>(nullable: true),
                    Gem3Id = table.Column<int>(nullable: true),
                    Gem4Id = table.Column<int>(nullable: true),
                    Gem5Id = table.Column<int>(nullable: true),
                    Gem6Id = table.Column<int>(nullable: true),
                    GlovesId = table.Column<int>(nullable: true),
                    Health = table.Column<int>(nullable: false),
                    HealthRegen = table.Column<int>(nullable: false),
                    HealthRegenCombat = table.Column<int>(nullable: false),
                    HeartId = table.Column<int>(nullable: true),
                    HongmoonLevel = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    LightningDamage = table.Column<int>(nullable: false),
                    MysticBadgeId = table.Column<int>(nullable: true),
                    NecklaceId = table.Column<int>(nullable: true),
                    PetId = table.Column<int>(nullable: true),
                    Piercing = table.Column<int>(nullable: false),
                    PvpAttackPower = table.Column<int>(nullable: false),
                    PvpDefense = table.Column<int>(nullable: false),
                    Recorded = table.Column<DateTime>(nullable: false),
                    RingId = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    ShadowDamage = table.Column<int>(nullable: false),
                    SoulBadgeId = table.Column<int>(nullable: true),
                    SoulId = table.Column<int>(nullable: true),
                    WeaponId = table.Column<int>(nullable: true),
                    WindDamage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Items_BeltId",
                        column: x => x.BeltId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_BraceletId",
                        column: x => x.BraceletId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Plogs_ClanMemberId",
                        column: x => x.ClanMemberId,
                        principalTable: "Plogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_Items_EarringId",
                        column: x => x.EarringId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem1Id",
                        column: x => x.Gem1Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem2Id",
                        column: x => x.Gem2Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem3Id",
                        column: x => x.Gem3Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem4Id",
                        column: x => x.Gem4Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem5Id",
                        column: x => x.Gem5Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_Gem6Id",
                        column: x => x.Gem6Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_GlovesId",
                        column: x => x.GlovesId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_HeartId",
                        column: x => x.HeartId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_MysticBadgeId",
                        column: x => x.MysticBadgeId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_NecklaceId",
                        column: x => x.NecklaceId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_PetId",
                        column: x => x.PetId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_RingId",
                        column: x => x.RingId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_SoulBadgeId",
                        column: x => x.SoulBadgeId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_SoulId",
                        column: x => x.SoulId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Items_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemType",
                table: "Items",
                column: "ItemType");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_BatchId",
                table: "Logs",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_BeltId",
                table: "Logs",
                column: "BeltId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_BraceletId",
                table: "Logs",
                column: "BraceletId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ClanMemberId",
                table: "Logs",
                column: "ClanMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_EarringId",
                table: "Logs",
                column: "EarringId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem1Id",
                table: "Logs",
                column: "Gem1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem2Id",
                table: "Logs",
                column: "Gem2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem3Id",
                table: "Logs",
                column: "Gem3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem4Id",
                table: "Logs",
                column: "Gem4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem5Id",
                table: "Logs",
                column: "Gem5Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Gem6Id",
                table: "Logs",
                column: "Gem6Id");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GlovesId",
                table: "Logs",
                column: "GlovesId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_HeartId",
                table: "Logs",
                column: "HeartId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_MysticBadgeId",
                table: "Logs",
                column: "MysticBadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_NecklaceId",
                table: "Logs",
                column: "NecklaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_PetId",
                table: "Logs",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Recorded",
                table: "Logs",
                column: "Recorded");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RingId",
                table: "Logs",
                column: "RingId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Score",
                table: "Logs",
                column: "Score");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_SoulBadgeId",
                table: "Logs",
                column: "SoulBadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_SoulId",
                table: "Logs",
                column: "SoulId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_WeaponId",
                table: "Logs",
                column: "WeaponId");

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
                name: "Items");

            migrationBuilder.DropTable(
                name: "Plogs");
        }
    }
}
