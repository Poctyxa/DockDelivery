using DockDelivery.Domain.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestDataLibrary;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using DockDelivery.Domain.Entities;
using System;

namespace DockDelivery.Mongo.Integration.Test.Helper
{
    internal static class TestHelper
    {
        public static void ConfigureBaseObjects(ref HttpClient httpClient,
             ref WebApplicationFactory<Startup> webHost,
             ref MongoDockDeliveryDbContext dbContext,
             ref MongoDbFixture dbFixture
            )
        {
            dbFixture = new MongoDbFixture();
            var dependencyLink = dbFixture;

            webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var mongoClient = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(IMongoDatabase));

                    services.Remove(mongoClient);
                    services.AddTransient<IMongoDatabase>(opts =>
                    {
                        return dependencyLink.mDatabase;
                    });
                });
            });

            httpClient = webHost.CreateClient();

            dbContext = (MongoDockDeliveryDbContext)webHost
            .Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService(typeof(IMongoDockDeliveryDbContext));
        }

        public static void BindDepartmentData(
            List<Department> departments,
            List<CargoType> cargoTypes,
            List<CargoSection> cargoSections,
            List<Cargo> cargos
            )
        {
            Random rnd = new Random();

            for (int i = 0; i < cargoSections.Count; i++)
            {
                cargoSections[i].CargoTypeId = cargoTypes[rnd.Next(0, cargoTypes.Count - 1)].Id;

                int departmentNumber;
                bool haveSectionWithType;

                do
                {
                    departmentNumber = rnd.Next(0, departments.Count - 1);
                    haveSectionWithType = departments[departmentNumber]
                        .CargoSections
                        .FirstOrDefault(cs => cs.CargoTypeId == cargoSections[i]
                        .CargoTypeId) != null;
                }
                while (haveSectionWithType);

                cargoSections[i].DepartmentId = departments[departmentNumber].Id;
            }

            for (int i = 0; i < cargos.Count; i++)
            {
                cargos[i].CargoSectionId = cargoSections[rnd.Next(0, cargoSections.Count - 1)].Id;
            }
        }
    }
}
