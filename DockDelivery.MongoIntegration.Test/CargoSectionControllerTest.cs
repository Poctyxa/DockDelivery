using AutoFixture;
using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.CargoSection;
using DockDelivery.Mongo.Integration.Test.Helper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestDataLibrary;

namespace DockDelivery.Mongo.Integration.Test
{
    public class CargoSectionControllerTest : IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly WebApplicationFactory<Startup> webHost;
        protected readonly MongoDockDeliveryDbContext dbContext;
        protected readonly MongoDbFixture dbFixture;

        public CargoSectionControllerTest()
        {
            TestHelper.ConfigureBaseObjects(ref this.httpClient, ref this.webHost, ref this.dbContext, ref this.dbFixture);
        }

        [Test]
        public async Task GetCargoSections_SendRequest_ShouldReturnCargoSections()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<CargoSection> cargoSections = fixture.Build<CargoSection>()
                .Without(cs => cs.Cargos)
                .Without(cs => cs.Department)
                .Without(cs => cs.CargoType)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<CargoSection>(15)
                .ToList();

            await dbContext.CargoSections.InsertManyAsync(cargoSections);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO_SECTION);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = System.Text.Json.JsonSerializer.Deserialize<List<CargoSection>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Positive(resultList.Count);
        }

        [Test]
        public async Task CreateSection_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var model = fixture.Build<CreateCargoSection>().Create<CreateCargoSection>();

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync(AppRoutes.API_CARGO_SECTION, requestBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task RemoveSection_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<CargoSection> cargoSections = fixture.Build<CargoSection>()
                .Without(cs => cs.Cargos)
                .Without(cs => cs.Department)
                .Without(cs => cs.CargoType)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<CargoSection>(15)
                .ToList();

            await dbContext.CargoSections.InsertManyAsync(cargoSections);

            // Act
            HttpResponseMessage response = await httpClient.DeleteAsync(AppRoutes.API_CARGO_SECTION + "/" + cargoSections.First().Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ReadSection_SendRequest_ShouldReturnOkAndSectionData()
        {
            // Arrange
            IFixture fixture = new Fixture();
            
            var expectedSection = fixture.Build<CargoSection>()
                .Without(cs => cs.Cargos)
                .Without(cs => cs.Department)
                .Without(cs => cs.CargoType)
                .With(cs => cs.Id, ObjectId.GenerateNewId().ToString())
                .With(cs => cs.DepartmentId, ObjectId.GenerateNewId().ToString())
                .With(cs => cs.CargoTypeId, ObjectId.GenerateNewId().ToString())
                .Create<CargoSection>();
            
            await dbContext.CargoSections.InsertOneAsync(expectedSection);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO_SECTION + "/" + expectedSection.Id);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultData = System.Text.Json.JsonSerializer.Deserialize<CargoSection>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedSection.DepartmentId, resultData.DepartmentId);
            Assert.AreEqual(expectedSection.CapacityLimit, resultData.CapacityLimit);
        }

        public void Dispose()
        {
            dbFixture.Dispose();
        }
    }
}
