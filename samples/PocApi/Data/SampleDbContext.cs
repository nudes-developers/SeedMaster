using Microsoft.EntityFrameworkCore;
using PocApi.Data.Domain;

namespace PocApi.Data
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
