using Microsoft.Extensions.Logging;
using Nudes.SeedMaster.Interfaces;
using PocApi.Data.Test;
using SeedMaster.Seeder;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PocApi.Data.Seeders
{
    public class TestSeeder : EfCoreSeeder<TestDbContext>
    {
        public TestSeeder(IServiceProvider serviceProvider, TestDbContext dbContext, ILogger<ISeeder<TestDbContext>> logger) : base(serviceProvider, dbContext, logger) { }

        public override Task Seed(params Assembly[] assemblies)
        {
            if (assemblies == null)
                assemblies = new Assembly[] { this.GetType().Assembly };
            else
                assemblies = assemblies.Append(this.GetType().Assembly).ToArray();

            return base.Seed(assemblies);
        }
    }
}
