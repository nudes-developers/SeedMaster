using Nudes.SeedMaster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocApi.Data.Sample
{
    public class Sample
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class SampleSeed : ISeed<SampleDbContext>
    {
        private readonly SampleDbContext dbContext;

        public SampleSeed(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
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
