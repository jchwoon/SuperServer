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
            var skillsConverter = new ValueConverter<Dictionary<int, int>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, (JsonSerializerOptions)null) ?? new Dictionary<int, int>()
            );

            var skillsComparer = new ValueComparer<Dictionary<int, int>>(
                (c1, c2) => c1 != null && c2 != null && c1.Count == c2.Count && !c1.Except(c2).Any(),
                c => c == null ? 0 : c.Aggregate(0, (hash, pair) => HashCode.Combine(hash, pair.Key, pair.Value)),
                c => c == null ? new Dictionary<int, int>() : c.ToDictionary(entry => entry.Key, entry => entry.Value)
            );

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
