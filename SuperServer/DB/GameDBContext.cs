using Microsoft.EntityFrameworkCore;
using SuperServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.DB
{
    public class GameDBContext : DbContext
    {
        public DbSet<DBHero> Heroes { get; set; }
        public DbSet<DBItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            ConfigManager.LoadConfigData();
            options.UseSqlServer(ConfigManager.Config.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBHero>()
                .OwnsOne(h => h.HeroStat);

            modelBuilder.Entity<DBItem>()
                .HasOne(i => i.OwnerDb)
                .WithMany(h => h.Items)
                .HasForeignKey(i => i.OwnerDbId)
                .IsRequired();
        }
    }
}
