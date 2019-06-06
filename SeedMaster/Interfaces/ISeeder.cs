using Microsoft.EntityFrameworkCore;
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
}
