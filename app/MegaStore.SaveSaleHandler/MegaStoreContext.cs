﻿using MegaStore.Helper;
using Microsoft.EntityFrameworkCore;

namespace MegaStore.SaveSaleHandler
{
    // This code is modified from https://github.com/sixeyed/docker-on-windows
    public class MegaStoreContext : DbContext
    {
        public DbSet<Sale> Sale { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Env.DbConnectionString);

        }

    }
}
