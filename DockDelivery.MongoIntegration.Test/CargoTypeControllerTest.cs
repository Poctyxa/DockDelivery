using DockDelivery.Domain.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TestDataLibrary;
using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;
using AutoFixture;
using DockDelivery.Domain.Entities;
using MongoDB.Bson;
using System.Net;
using Newtonsoft.Json;
using DockDelivery.Mongo.Integration.Test.Helper;

namespace DockDelivery.Mongo.Integration.Test
{
    public class CargoTypeControllerTest : IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly WebApplicationFactory<Startup> webHost;
        protected readonly MongoDockDeliveryDbContext dbContext;
        protected readonly MongoDbFixture dbFixture;

        public CargoTypeControllerTest()
        {
            TestHelper.ConfigureBaseObjects(ref this.httpClient, ref this.webHost, ref this.dbContext, ref this.dbFixture);
        }

        [Test]
        public async Task GetCargoType_SendRequest_ShouldReturnCargoTypes()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<CargoType> cargoTypes = fixture
                .Build<CargoType>()
                .With(ct => ct.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<CargoType>(15)
                .ToList();

            await dbContext.CargoTypes.InsertManyAsync(cargoTypes);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO_TYPE);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = System.Text.Json.JsonSerializer.Deserialize<List<CargoType>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Positive(resultList.Count);
        }

        [Test]
        public async Task CreateCargoType_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var model = fixture
                .Build<CargoType>()
                .With(ct => ct.Id, ObjectId.GenerateNewId().ToString())
                .Create<CargoType>();

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync(AppRoutes.API_CARGO_TYPE, requestBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task RemoveCargoType_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<CargoType> cargoTypes = fixture
                .Build<CargoType>()
                .With(ct => ct.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany<CargoType>(15)
                .ToList();

            await dbContext.CargoTypes.InsertManyAsync(cargoTypes);

            // Act
            HttpResponseMessage response = await httpClient.DeleteAsync(AppRoutes.API_CARGO_TYPE + "/" + cargoTypes.First().Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ReadCargoType_SendRequest_ShouldReturnOkAndSectionData()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var expectedCargoType = fixture
                .Build<CargoType>()
                .With(ct => ct.Id, ObjectId.GenerateNewId().ToString())
                .Create<CargoType>();

            await dbContext.CargoTypes.InsertOneAsync(expectedCargoType);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_CARGO_TYPE + "/" + expectedCargoType.Id);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultData = System.Text.Json.JsonSerializer.Deserialize<CargoType>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedCargoType.TypeName, resultData.TypeName);
        }

        public void Dispose()
        {
            dbFixture.Dispose();
        }
    }
}
