using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ASWA.Core.Interfaces;
using ASWA.Infra.Data;
using ASWA.Infra.Services.Storage;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using ASWA.Web.Interfaces;
using ASWA.Web.Services;

namespace ASWA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(
                    options, Environment.GetEnvironmentVariable("MSSQLDB_CONNECTION")));
            
            services.Configure<TelemetryConfiguration>((o) =>
            {
                o.InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
                o.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            });
            services.Configure<BlobStorageOptions>((o) =>
            {
                o.ConnectionString = Environment.GetEnvironmentVariable("BLOB_STORAGE_CONNECTION");
                o.Containers = new Dictionary<string, string> { { "your_container_key", "your_container_name" } };
            });
            services.AddSingleton<IBlobStorageService, BlobStorageService>();

            // Our services
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerViewModelService, CustomerViewModelService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
