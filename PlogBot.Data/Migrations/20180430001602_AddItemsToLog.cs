using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlogBot.Data.Migrations
{
    public partial class AddItemsToLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BeltId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BraceletId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EarringId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem1Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem2Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem3Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem4Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem5Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gem6Id",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GlovesId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeartId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MysticBadgeId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NecklaceId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RingId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoulBadgeId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoulId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeaponId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Items_ItemType",
                table: "Items",
                column: "ItemType");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_BeltId",
                table: "Logs",
                column: "BeltId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_BraceletId",
                table: "Logs",
                column: "BraceletId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_EarringId",
                table: "Logs",
                column: "EarringId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem1Id",
                table: "Logs",
                column: "Gem1Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem2Id",
                table: "Logs",
                column: "Gem2Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem3Id",
                table: "Logs",
                column: "Gem3Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem4Id",
                table: "Logs",
                column: "Gem4Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem5Id",
                table: "Logs",
                column: "Gem5Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_Gem6Id",
                table: "Logs",
                column: "Gem6Id",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_GlovesId",
                table: "Logs",
                column: "GlovesId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_HeartId",
                table: "Logs",
                column: "HeartId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_MysticBadgeId",
                table: "Logs",
                column: "MysticBadgeId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_NecklaceId",
                table: "Logs",
                column: "NecklaceId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_PetId",
                table: "Logs",
                column: "PetId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_RingId",
                table: "Logs",
                column: "RingId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_SoulBadgeId",
                table: "Logs",
                column: "SoulBadgeId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_SoulId",
                table: "Logs",
                column: "SoulId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Items_WeaponId",
                table: "Logs",
                column: "WeaponId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_BeltId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_BraceletId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_EarringId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem1Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem2Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem3Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem4Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem5Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_Gem6Id",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_GlovesId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_HeartId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_MysticBadgeId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_NecklaceId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_PetId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_RingId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_SoulBadgeId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_SoulId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Items_WeaponId",
                table: "Logs");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Logs_BatchId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_BeltId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_BraceletId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_EarringId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem1Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem2Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem3Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem4Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem5Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Gem6Id",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GlovesId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_HeartId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_MysticBadgeId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_NecklaceId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_PetId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_RingId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Score",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_SoulBadgeId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_SoulId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_WeaponId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "BeltId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "BraceletId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "EarringId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem1Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem2Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem3Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem4Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem5Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Gem6Id",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "GlovesId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HeartId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "MysticBadgeId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "NecklaceId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RingId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "SoulBadgeId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "SoulId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "WeaponId",
                table: "Logs");
        }
    }
}
