﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using PlogBot.Data;

namespace PlogBot.Data.Migrations
{
    [DbContext(typeof(PlogDbContext))]
    [Migration("20180609042339_AddingAlerts")]
    partial class AddingAlerts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview2-30571");

            modelBuilder.Entity("PlogBot.Data.Entities.Alert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("ChannelId");

                    b.Property<int?>("Day");

                    b.Property<string>("Description");

                    b.Property<ulong>("DiscordUserId");

                    b.Property<DateTime?>("LastProcessed");

                    b.Property<string>("Name");

                    b.Property<string>("Roles");

                    b.Property<int>("Time");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("PlogBot.Data.Entities.ClanMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("Class");

                    b.Property<DateTime>("Created");

                    b.Property<ulong?>("DiscordId");

                    b.Property<string>("ImageUrl");

                    b.Property<Guid?>("MainId");

                    b.Property<DateTime?>("Modified");

                    b.Property<string>("Name");

                    b.Property<string>("RealName");

                    b.HasKey("Id");

                    b.HasIndex("MainId");

                    b.ToTable("Plogs");
                });

            modelBuilder.Entity("PlogBot.Data.Entities.ClanMemberStatLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Accuracy");

                    b.Property<int>("AdditionalDamage");

                    b.Property<int>("AttackPower");

                    b.Property<int>("BatchId");

                    b.Property<int?>("BeltId");

                    b.Property<int>("Block");

                    b.Property<int>("BossAttackPower");

                    b.Property<int>("BossDefense");

                    b.Property<int?>("BraceletId");

                    b.Property<Guid>("ClanMemberId");

                    b.Property<int>("Concentration");

                    b.Property<int>("Critical");

                    b.Property<int>("CriticalDamage");

                    b.Property<int>("CriticalDefense");

                    b.Property<int>("DamageReduction");

                    b.Property<int>("DebuffDamage");

                    b.Property<int>("DebuffDefense");

                    b.Property<int>("Defense");

                    b.Property<int?>("EarringId");

                    b.Property<int>("EarthDamage");

                    b.Property<int>("Evasion");

                    b.Property<int>("FlameDamage");

                    b.Property<int>("FrostDamage");

                    b.Property<int?>("Gem1Id");

                    b.Property<int?>("Gem2Id");

                    b.Property<int?>("Gem3Id");

                    b.Property<int?>("Gem4Id");

                    b.Property<int?>("Gem5Id");

                    b.Property<int?>("Gem6Id");

                    b.Property<int?>("GlovesId");

                    b.Property<int>("Health");

                    b.Property<int>("HealthRegen");

                    b.Property<int>("HealthRegenCombat");

                    b.Property<int?>("HeartId");

                    b.Property<int>("HongmoonLevel");

                    b.Property<int>("Level");

                    b.Property<int>("LightningDamage");

                    b.Property<int?>("MysticBadgeId");

                    b.Property<int?>("NecklaceId");

                    b.Property<int?>("PetId");

                    b.Property<int>("Piercing");

                    b.Property<int>("PvpAttackPower");

                    b.Property<int>("PvpDefense");

                    b.Property<DateTime>("Recorded");

                    b.Property<int?>("RingId");

                    b.Property<int>("Score");

                    b.Property<int>("ShadowDamage");

                    b.Property<int?>("SoulBadgeId");

                    b.Property<int?>("SoulId");

                    b.Property<int?>("WeaponId");

                    b.Property<int>("WindDamage");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.HasIndex("BeltId");

                    b.HasIndex("BraceletId");

                    b.HasIndex("ClanMemberId");

                    b.HasIndex("EarringId");

                    b.HasIndex("Gem1Id");

                    b.HasIndex("Gem2Id");

                    b.HasIndex("Gem3Id");

                    b.HasIndex("Gem4Id");

                    b.HasIndex("Gem5Id");

                    b.HasIndex("Gem6Id");

                    b.HasIndex("GlovesId");

                    b.HasIndex("HeartId");

                    b.HasIndex("MysticBadgeId");

                    b.HasIndex("NecklaceId");

                    b.HasIndex("PetId");

                    b.HasIndex("Recorded");

                    b.HasIndex("RingId");

                    b.HasIndex("Score");

                    b.HasIndex("SoulBadgeId");

                    b.HasIndex("SoulId");

                    b.HasIndex("WeaponId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("PlogBot.Data.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImgUrl");

                    b.Property<int>("ItemType");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ItemType");

                    b.HasIndex("Name");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PlogBot.Data.Entities.ClanMember", b =>
                {
                    b.HasOne("PlogBot.Data.Entities.ClanMember", "Main")
                        .WithMany()
                        .HasForeignKey("MainId");
                });

            modelBuilder.Entity("PlogBot.Data.Entities.ClanMemberStatLog", b =>
                {
                    b.HasOne("PlogBot.Data.Entities.Item", "Belt")
                        .WithMany()
                        .HasForeignKey("BeltId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Bracelet")
                        .WithMany()
                        .HasForeignKey("BraceletId");

                    b.HasOne("PlogBot.Data.Entities.ClanMember", "ClanMember")
                        .WithMany("ClanMemberStatLogs")
                        .HasForeignKey("ClanMemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PlogBot.Data.Entities.Item", "Earring")
                        .WithMany()
                        .HasForeignKey("EarringId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem1")
                        .WithMany()
                        .HasForeignKey("Gem1Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem2")
                        .WithMany()
                        .HasForeignKey("Gem2Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem3")
                        .WithMany()
                        .HasForeignKey("Gem3Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem4")
                        .WithMany()
                        .HasForeignKey("Gem4Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem5")
                        .WithMany()
                        .HasForeignKey("Gem5Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gem6")
                        .WithMany()
                        .HasForeignKey("Gem6Id");

                    b.HasOne("PlogBot.Data.Entities.Item", "Gloves")
                        .WithMany()
                        .HasForeignKey("GlovesId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Heart")
                        .WithMany()
                        .HasForeignKey("HeartId");

                    b.HasOne("PlogBot.Data.Entities.Item", "MysticBadge")
                        .WithMany()
                        .HasForeignKey("MysticBadgeId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Necklace")
                        .WithMany()
                        .HasForeignKey("NecklaceId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Pet")
                        .WithMany()
                        .HasForeignKey("PetId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Ring")
                        .WithMany()
                        .HasForeignKey("RingId");

                    b.HasOne("PlogBot.Data.Entities.Item", "SoulBadge")
                        .WithMany()
                        .HasForeignKey("SoulBadgeId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Soul")
                        .WithMany()
                        .HasForeignKey("SoulId");

                    b.HasOne("PlogBot.Data.Entities.Item", "Weapon")
                        .WithMany()
                        .HasForeignKey("WeaponId");
                });
#pragma warning restore 612, 618
        }
    }
}
