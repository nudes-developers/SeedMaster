using Nudes.SeedMaster.Interfaces;
using PocApi.Data.Domain;
using System;
using System.Threading.Tasks;

namespace PocApi.Data.Seeders
{
    public class TestSeeder : BaseSeed<TestDbContext>
    {
        public override async Task Seed(TestDbContext dbContext)
        {
            dbContext.Add(new Test
            {
                Id = Guid.NewGuid(),
                Title = "Title 3.0"
            });

            await Task.CompletedTask;
        }
    }
}
