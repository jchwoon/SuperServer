﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            ConfigManager.LoadConfigData();
            options.UseSqlServer(ConfigManager.Config.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}