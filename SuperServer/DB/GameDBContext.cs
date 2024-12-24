using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuperServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            var skillsConverter = new ValueConverter<List<int>, string>(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
            v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null) ?? new List<int>());

            var skillsComparer = new ValueComparer<List<int>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

            modelBuilder.Entity<DBHero>()
                .Property(h => h.Skills)
                .HasConversion(skillsConverter)
                .Metadata.SetValueComparer(skillsComparer);


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
