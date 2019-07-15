using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nudes.SeedMaster.Interfaces;

namespace PocApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<ISeeder>();
                await seeder.Run();
            }

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
