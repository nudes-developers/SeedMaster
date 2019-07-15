using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocApi.Data.Sample
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Sample>().HasKey(d => d.Id);
            mb.Entity<Sample>().Property(d => d.Name).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}
