using Nudes.SeedMaster.Interfaces;
using System;
using System.Threading.Tasks;
using PocApi.Data.Domain;

namespace PocApi.Data.Seeders
{
    public class SampleSeed : BaseSeed<SampleDbContext>
    {
        public override async Task Seed(SampleDbContext dbContext)
        {
            dbContext.Samples.Add(new Sample
            {
                Id = Guid.NewGuid(),
                Name = "Sample"
            });

            await Task.CompletedTask;
        }
    }
}
