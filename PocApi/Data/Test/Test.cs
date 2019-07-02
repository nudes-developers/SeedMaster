using Nudes.SeedMaster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocApi.Data.Test
{
    public class Test
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }

    public class TestSeeder : ISeed<TestDbContext>
    {
        private readonly TestDbContext dbContext;

        public TestSeeder(TestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
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
