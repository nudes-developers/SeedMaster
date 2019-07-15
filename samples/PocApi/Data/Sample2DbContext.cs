using Microsoft.EntityFrameworkCore;
using PocApi.Data.Domain;

namespace PocApi.Data
{
    public class Sample2DbContext : DbContext
    {
        public DbSet<Sample2> Samples2 { get; set; }

        public Sample2DbContext(DbContextOptions<Sample2DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Sample>().HasKey(d => d.Id);
            mb.Entity<Sample>().Property(d => d.Name).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}
