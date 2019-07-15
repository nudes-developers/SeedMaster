using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nudes.SeedMaster;
using Nudes.SeedMaster.Interfaces;
using Nudes.SeedMaster.Seeder;
using PocApi.Data;

namespace PocApi
{
    public class Startup : IStartup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment enviroment)
        {
            var configurationBuilder = new ConfigurationBuilder().
                            SetBasePath(enviroment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
            Environment = enviroment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            #region Database configuration
            // Adding DbContext For Sample and Sample 2 using the same database
            services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Sample")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            services.AddDbContext<Sample2DbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Sample")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            // Adding DbContext For Test in another database
            services.AddDbContext<TestDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Test")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);

            // Adding all dbcontext as a service retrieved for DbContext so we can call all of them as IEnumerable<DbContext>
            services.AddScoped<DbContext>(provider => provider.GetService<SampleDbContext>());
            services.AddScoped<DbContext>(provider => provider.GetService<Sample2DbContext>());
            services.AddScoped<DbContext>(provider => provider.GetService<TestDbContext>());

            // Scanning our assembly and adding all ISeed
            SeedScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                .ForEach(d => services.AddScoped(d.InterfaceType, d.ValidatorType));

            // Adding our EfCoreSeeder service
            services.AddScoped<ISeeder, EfCoreSeeder>();
            #endregion

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {

        }
    }
}
