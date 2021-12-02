using Microsoft.EntityFrameworkCore;
using PocApi.Data.Domain;

namespace PocApi.Data
{
    public class TestDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }

        public DbSet<ManyLeft> Lefts { get; set; }

        public DbSet<ManyRight> Rights { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Test>().HasKey(d => d.Id);
            mb.Entity<Test>().Property(d => d.Title).IsRequired();

            mb.Entity<ManyLeft>(left =>
            {
                left.HasKey(d => d.Id);

                left.HasMany(d => d.Rights)
                    .WithMany(d => d.Lefts);
            });

            mb.Entity<ManyRight>(right =>
            {
                right.HasKey(d => d.Id);
            });

            base.OnModelCreating(mb);
        }
    }
}
