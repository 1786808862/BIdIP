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

        public DbSet<BidIP.Models.BidIpModel> BidIpModel { get; set; } = default!;
    }
}
