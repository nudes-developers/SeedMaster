using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nudes.SeedMaster.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SeedMaster.Seeder
{
    public class EfCoreSeeder<TDbContext> : ISeeder<TDbContext> where TDbContext : DbContext
    {
        private readonly IServiceProvider serviceProvider;
        private readonly TDbContext dbContext;
        private readonly ILogger<EfCoreSeeder<TDbContext>> logger;

        public EfCoreSeeder(IServiceProvider serviceProvider, TDbContext dbContext, ILogger<EfCoreSeeder<TDbContext>> logger)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public virtual async Task CleanDb()
        {
            #region Droping

            logger.LogInformation("Starting database dropping");

            await dbContext.Database.EnsureDeletedAsync();

            logger.LogInformation($"Database dropped");

            #endregion

            #region Creation

            logger.LogInformation("Starting database creation");
            
            await dbContext.Database.EnsureCreatedAsync();

            logger.LogInformation("The Database was created");

            #endregion
        }

        public virtual async Task Seed(params Assembly[] assemblies)
        {
            logger.LogInformation("Starting seed");

            await serviceProvider.SeedAllInto<TDbContext>(dbContext, assemblies);

            logger.LogInformation("Seed finalized");
        }

        public virtual async Task Commit()
        {
            logger.LogInformation("Starting commit");

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Commit finalized");
        }

        public virtual async Task Run()
        {
            await CleanDb();
            await Seed();
            await Commit();
        }

        public virtual void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
