using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocApi.Data.Test
{
    public class TestDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Test>().HasKey(d => d.Id);
            mb.Entity<Test>().Property(d => d.Title).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}
