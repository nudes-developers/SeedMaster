using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Nudes.SeedMaster.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nudes.SeedMaster.Seeder
{
    /// <summary>
    /// EF Core Implementation of seed strategy
    /// It should rely on DependencyInjection to acquire DbContexts and ISeeds
    /// </summary>
    public class EfCoreSeeder : ISeeder
    {
        private readonly IEnumerable<DbContext> contexts;
        private readonly IEnumerable<ISeed> seeds;
        private readonly ILogger<EfCoreSeeder> logger;

        public EfCoreSeeder(IEnumerable<DbContext> contexts, IEnumerable<ISeed> seeds, ILogger<EfCoreSeeder> logger)
        {
            this.contexts = contexts;
            this.seeds = seeds;
            this.logger = logger;
        }

        public virtual async Task Clean()
        {
            #region Droping

            logger?.LogInformation("Cleaning started");

            foreach (var db in contexts)
                await CleanDb(db);

            logger?.LogInformation("Database ended");

            #endregion
        }

        protected virtual async Task CleanDb(DbContext db)
        {
            logger?.LogInformation($"Cleaning context {db}");

            foreach (var type in db.Model.GetEntityTypes())
                await CleanEntity(db, type);
        }

        protected virtual async Task CleanEntity(DbContext db, IEntityType type)
        {
            logger?.LogInformation($"Cleaning entity {type.Name}");

            var boxedDbSet = db.GetType().GetMethod("Set").MakeGenericMethod(type.ClrType).Invoke(db, null);
            var dbSet = boxedDbSet as IQueryable<object>;
            db.RemoveRange(await dbSet.ToListAsync());
        }

        public virtual async Task Seed()
        {
            logger?.LogInformation("Starting seed");

            foreach (var db in contexts)
            {
                var dbseeds = seeds.Where(d => d.GetType().GetTypeInfo().ImplementedInterfaces.Any(f =>f.IsGenericType && f.GetGenericTypeDefinition() == typeof(ISeed<>) && f.GenericTypeArguments.Any(g => g == db.GetType())));
                foreach (var seed in dbseeds)
                {
                    logger?.LogInformation($"seeding {seed} into {db}");
                    await seed.Seed(db);
                }
            }

            logger?.LogInformation("Seed finalized");
        }

        public virtual async Task Commit()
        {
            logger?.LogInformation("Starting commit");

            foreach (var db in contexts)
            {
                logger?.LogInformation($"Commiting changes to {db}");
                await db.SaveChangesAsync();
            }

            logger?.LogInformation("Commit finalized");
        }

        public virtual async Task Run()
        {
            await Clean();
            await Seed();
            await Commit();
        }

        public virtual void Dispose()
        {
            foreach (var db in contexts)
                db?.Dispose();
        }
    }
}
