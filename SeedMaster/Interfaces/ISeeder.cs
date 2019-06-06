using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Nudes.SeedMaster.Interfaces
{
    public interface ISeeder<TDbContext> : IDisposable where TDbContext : DbContext
    {
        Task CleanDb();
        Task Seed(params Assembly[] assemblies);
        Task Commit();
        Task Run();
    }   

    public static class SeederExtensions
    {
        public static IServiceCollection AddSeeder<TDbContext, TImplementation>(this IServiceCollection services) where TDbContext : DbContext
                                                                                                           where TImplementation : class, ISeeder<TDbContext> 
            => services.AddTransient<ISeeder<TDbContext>, TImplementation>();
    }
}
