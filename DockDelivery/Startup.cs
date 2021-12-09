using AspNetCore.RouteAnalyzer;
using DockDelivery.Configuration;
using DockDelivery.Domain.Context;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.Repositories.Mongo;
using DockDelivery.Domain.UoW;
using DockDelivery.Domain.UoW.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockDelivery
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
            services.AddControllers()
                .AddJsonOptions(
                options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddDbContext<IDockDeliveryDbContext, DockDeliveryDbContext>(opts => {
                opts.UseSqlServer(Configuration["ConnectionStrings:DockDeliveryDb"]);
            });
            services.AddTransient<IDockDeliveryUnitOfWork, DockDeliveryUnitOfWork>();


            services.AddTransient<IMongoDockDeliveryDbContext, MongoDockDeliveryDbContext>();
            services.AddTransient<IMongoDockDeliveryUnitOfWork, MongoDockDeliveryUnitOfWork>();

            BusinessServiceConfiguration.AddBusinessServiceConfig(services);

            services.AddTransient<IMongoDatabase>(opts =>
            {
                return (new MongoClient(Configuration["ConnectionStrings:DockDeliveryMongoDb"]))
                .GetDatabase("DockDelivery");
            });

            services.AddAutoMapper(typeof(Startup)); 
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
