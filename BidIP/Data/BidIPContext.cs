using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BidIP.Models;

namespace BidIP.Data
{
    public class BidIPContext : DbContext
    {
        public BidIPContext (DbContextOptions<BidIPContext> options)
            : base(options)
        {
        }

        public DbSet<BidIP.Models.MachineDetailInfo> MachineDetailInfo { get; set; } = default!;
        public DbSet<BidIP.Models.MachineCategory> MachineCategory { get; set; } = default!;
        public DbSet<BidIP.Models.CustomerInfo> CustomerInfo { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置一对多关系
            modelBuilder.Entity<MachineCategory>()
                .HasMany(m => m.MachineDetailInfo)
                .WithOne(m => m.MachineCategory)
                .HasForeignKey(m => m.MachineCategoryId);
        }

    }
}
