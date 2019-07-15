using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nudes.SeedMaster.Interfaces;
using Nudes.SeedMaster.Seeder;
using PocApi.Data.Sample;
using PocApi.Data.Seeders;
using PocApi.Data.Test;

namespace PocApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                await RunSampleSeed(scope.ServiceProvider);
                await RunTestSeed(scope.ServiceProvider);
            }

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static async Task RunSampleSeed(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ISeeder<SampleDbContext>>>();
            var dbContext = serviceProvider.GetRequiredService<SampleDbContext>();

            using (SampleSeeder seeder = new SampleSeeder(serviceProvider, dbContext, logger))
                await seeder.Run();
        }
        private static async Task RunTestSeed(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ISeeder<TestDbContext>>>();
            var dbContext = serviceProvider.GetRequiredService<TestDbContext>();

            using (Data.Seeders.TestSeeder seeder = new Data.Seeders.TestSeeder(serviceProvider, dbContext, logger))
                await seeder.Run();
        }
    }
}
