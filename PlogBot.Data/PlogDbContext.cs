using Microsoft.EntityFrameworkCore;
using PlogBot.Data.Entities;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PlogBot.Data
{
    public class PlogDbContext : DbContext
    {
        public DbSet<ClanMember> Plogs { get; set; }
        public DbSet<ClanMemberStatLog> Logs { get; set; }
        public DbSet<Item> Items { get; set; }

        public PlogDbContext() { }

        public PlogDbContext(DbContextOptions<PlogDbContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteFilePath = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home"), @"PlogBot\plog.db");
            optionsBuilder.UseSqlite($"Data Source={sqliteFilePath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClanMemberStatLog>().HasIndex(l => l.Recorded);
            builder.Entity<ClanMemberStatLog>().HasIndex(l => l.BatchId);
            builder.Entity<ClanMemberStatLog>().HasIndex(l => l.Score);
            builder.Entity<Item>().HasIndex(i => i.ItemType);
            builder.Entity<Item>().HasIndex(i => i.Name);
        }
    }
}
