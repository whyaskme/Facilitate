using Facilitate.Libraries.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class FacilitateDbContext : DbContext
    {
        public DbSet<Lead> Leads { get; set; }

        public FacilitateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Lead>(entity =>
            {
                entity.ToCollection("Quote");
                entity.HasKey(f => f._id);
                entity.HasIndex(f => f.timestamp);
                entity.HasIndex(f => f.Trade);
                entity.HasIndex(f => f.status);
            });
        }

    }
}
