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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqliteFilePath = Path.Combine(Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "Home"), @"PlogBot\plog.db");
            optionsBuilder.UseSqlite($"Data Source={sqliteFilePath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClanMemberStatLog>().HasIndex(l => l.Recorded);
        }
    }
}
