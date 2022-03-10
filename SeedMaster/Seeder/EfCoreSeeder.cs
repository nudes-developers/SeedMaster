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

            logger?.LogInformation("Cleaning ended");

            #endregion
        }

        protected virtual async Task CleanDb(DbContext db)
        {
            logger?.LogInformation("Cleaning context {db}",db);

            foreach (var type in db.Model.GetEntityTypes())
                await CleanEntity(db, type);
        }

        protected virtual async Task CleanEntity(DbContext db, IEntityType type)
        {
            logger?.LogInformation("Cleaning entity {typeName}", type.Name);
            if (type.ClrType == typeof(Dictionary<string, object>))
            {
                logger?.LogWarning("type {typeName} is a many to many, skipping", type.Name);
                return;
            }

            if (type.IsOwned())
            {
                logger?.LogWarning("type {typeName} is a many to many, skipping", type.Name);
                return;
            }

            var boxedDbSet = db.GetType().GetMethods()
                                         .Where(d => d.Name == "Set")
                                         .FirstOrDefault(d => d.IsGenericMethod)
                                         .MakeGenericMethod(type.ClrType).Invoke(db, null);
                                         
            var dbSet = boxedDbSet as IQueryable<object>;
            db.RemoveRange(await dbSet.IgnoreQueryFilters().ToListAsync());
        }

        public virtual async Task Seed()
        {
            logger?.LogInformation("Starting seed");

            foreach (var db in contexts)
            {
                var dbseeds = seeds.Where(d => d.GetType().GetTypeInfo().ImplementedInterfaces.Any(f =>f.IsGenericType && f.GetGenericTypeDefinition() == typeof(ISeed<>) && f.GenericTypeArguments.Any(g => g == db.GetType())));
                foreach (var seed in dbseeds)
                {
                    logger?.LogInformation("seeding {seed} into {db}", seed, db);
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
                logger?.LogInformation("Commiting changes to {db}", db);
                await db.SaveChangesAsync();
            }

            logger?.LogInformation("Commit finalized");
        }

        public virtual async Task Run()
        {
            await Clean();
            await Commit();

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
