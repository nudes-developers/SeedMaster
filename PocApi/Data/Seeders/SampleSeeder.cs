using Microsoft.Extensions.Logging;
using Nudes.SeedMaster.Interfaces;
using PocApi.Data.Sample;
using SeedMaster.Seeder;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PocApi.Data.Seeders
{
    public class SampleSeeder : EfCoreSeeder<SampleDbContext>
    {
        public SampleSeeder(IServiceProvider serviceProvider, SampleDbContext dbContext, ILogger<ISeeder<SampleDbContext>> logger) : base(serviceProvider, dbContext, logger) { }

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
