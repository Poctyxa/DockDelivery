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
    public class CargoControllerTest : IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly WebApplicationFactory<Startup> webHost;
        protected readonly MongoDockDeliveryDbContext dbContext;
        protected readonly MongoDbFixture dbFixture;

        public CargoControllerTest()
        {
            TestHelper.ConfigureBaseObjects(ref this.httpClient, ref this.webHost, ref this.dbContext, ref this.dbFixture);
        }

        [Test]
        public async Task GetCargo_SendRequest_ShouldReturnCargoSections()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<Cargo> cargos = fixture.Build<Cargo>()
                .Without(cs => cs.CargoSection)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<Cargo>(15)
                .ToList();

            await dbContext.Cargos.InsertManyAsync(cargos);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = System.Text.Json.JsonSerializer.Deserialize<List<Cargo>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Positive(resultList.Count);
        }

        [Test]
        public async Task CreateCargo_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var model = fixture.Build<Cargo>()
                .Without(cs => cs.CargoSection)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .Create();

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync(AppRoutes.API_CARGO, requestBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task RemoveCargo_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<Cargo> cargos = fixture.Build<Cargo>()
                .Without(cs => cs.CargoSection)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<Cargo>(15)
                .ToList();

            await dbContext.Cargos.InsertManyAsync(cargos);

            // Act
            HttpResponseMessage response = await httpClient.DeleteAsync(AppRoutes.API_CARGO + "/" + cargos.First().Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ReadSection_SendRequest_ShouldReturnOkAndSectionData()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var expectedSection = fixture.Build<Cargo>()
                .Without(cs => cs.CargoSection)
                .With(cs => cs.Id, () => ObjectId.GenerateNewId().ToString())
                .Create();

            await dbContext.Cargos.InsertOneAsync(expectedSection);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO  + "/" + expectedSection.Id);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultData = System.Text.Json.JsonSerializer.Deserialize<Cargo>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedSection.Capacity, resultData.Capacity);
            Assert.AreEqual(expectedSection.Owner, resultData.Owner);
            Assert.AreEqual(expectedSection.Weight, resultData.Weight);
        }

        public void Dispose()
        {
            dbFixture.Dispose();
        }
    }
}
