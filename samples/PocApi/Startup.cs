using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocApi.Data.Sample;
using PocApi.Data.Test;

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
            services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Sample")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            services.AddDbContext<TestDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Test")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            #endregion

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            
        }

    }
}
