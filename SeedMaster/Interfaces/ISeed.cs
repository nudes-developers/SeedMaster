using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nudes.SeedMaster.Interfaces
{
    public interface ISeed<TDbContext> where TDbContext : DbContext
    {
        Task Seed();
    }

    public static class SeedExtensions
    {
        public static async Task SeedAllInto<TDbContext>(this IServiceProvider serviceProvider, TDbContext dbContext, params Assembly[] assemblies) where TDbContext : DbContext
        {
            if (assemblies == null || !assemblies.Any())
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var allSeeds = assemblies.SelectMany(d => d.GetTypes())
                                     .Where(d => typeof(ISeed<TDbContext>).IsAssignableFrom(d))
                                     .Where(d => d.IsClass);

            foreach (var seedClass in allSeeds)
            {
                var instance = ActivatorUtilities.CreateInstance(serviceProvider, seedClass, new object[] { dbContext }) as ISeed<TDbContext>;
                await instance.Seed();
            }
        }
    }
}