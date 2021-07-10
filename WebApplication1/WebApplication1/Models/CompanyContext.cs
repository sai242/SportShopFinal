using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class CompanyContext: DbContext
    {
        public CompanyContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Cart> Cart { get; set; }

        
    }
}
