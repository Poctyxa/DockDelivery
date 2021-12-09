using AutoFixture;
using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.Department;
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
    internal class DepartmentControllerTest : IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly WebApplicationFactory<Startup> webHost;
        protected readonly MongoDockDeliveryDbContext dbContext;
        protected readonly MongoDbFixture dbFixture;

        public DepartmentControllerTest()
        {
            TestHelper.ConfigureBaseObjects(ref this.httpClient, ref this.webHost, ref this.dbContext, ref this.dbFixture);
        }

        [Test]
        public async Task GetDepartments_SendRequest_ShouldReturnDepartments()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<Department> departments = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany(10)
                .ToList();

            await dbContext.Departments.InsertManyAsync(departments);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_DEPARTMENT);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = System.Text.Json.JsonSerializer.Deserialize<List<Department>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task AssignDepart_SendRequest_ShouldClearCargoAndChangeDepartmentDates()
        {
            // Arrange
            IFixture fixture = new Fixture();

            Department expected = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .With(d => d.LastSending, () => DateTime.Now.AddDays(-45))
                .With(d => d.NextSending, () => DateTime.Now.AddDays(+45))
                .Create();

            await dbContext.Departments.InsertOneAsync(expected);

            var model = new AssignDepartModel() { DepartmentId = expected.Id };
            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync(AppRoutes.API_DEPARTMENT + AppRoutes.ASSIGN_DEPART, requestBody);
            var resultContent = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Cargo is departed.", resultContent);
        }

        [Test]
        public async Task GetCapableDepartments_SendRequest_ShouldReturnDepartments()
        {
            // Arrange
            IFixture fixture = new Fixture();

            List<Department> departments = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany(10)
                .ToList();

            List<CargoType> cargoTypes = fixture.Build<CargoType>()
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .CreateMany(10)
                .ToList();

            List<CargoSection> cargoSections = fixture.Build<CargoSection>()
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .Without(cs => cs.Cargos)
                .Without(cs => cs.Department)
                .Without(cs => cs.CargoType)
                .CreateMany(50)
                .ToList();

            List<Cargo> cargos = fixture.Build<Cargo>()
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .Without(cs => cs.CargoSection)
                .CreateMany(150)
                .ToList();

            TestHelper.BindDepartmentData(departments, cargoTypes, cargoSections, cargos);

            await dbContext.Departments.InsertManyAsync(departments);
            await dbContext.CargoSections.InsertManyAsync(cargoSections);
            await dbContext.CargoTypes.InsertManyAsync(cargoTypes);
            await dbContext.Cargos.InsertManyAsync(cargos);

            DateTime testDate = Convert.ToDateTime("2020-11-01");
            string testCargoTypeId = cargoSections.First().CargoTypeId;
            double testWeigth = 27;
            double testCapacity = 14;

            string query = AppRoutes.API_DEPARTMENT + 
                AppRoutes.CAPABLE_DEPARTMENTS(testDate, testWeigth, testCapacity, testCargoTypeId.ToString());

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(query);

            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = System.Text.Json.JsonSerializer.Deserialize<List<Department>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(resultList);
        }


        [Test]
        public async Task CreateDepartment_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var model = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .With(d => d.LastSending, () => DateTime.Now.AddDays(-45))
                .With(d => d.NextSending, () => DateTime.Now.AddDays(+45))
                .Create();

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await httpClient.PostAsync(AppRoutes.API_DEPARTMENT, requestBody);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task RemoveDepartment_SendRequest_ShouldReturnOk()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var expectedDepartment = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .With(d => d.LastSending, () => DateTime.Now.AddDays(-45))
                .With(d => d.NextSending, () => DateTime.Now.AddDays(+45))
                .Create();

            await dbContext.Departments.InsertOneAsync(expectedDepartment);

            // Act
            HttpResponseMessage response = await httpClient.DeleteAsync(AppRoutes.API_DEPARTMENT + "/" + expectedDepartment.Id);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ReadDepartment_SendRequest_ShouldReturnOkAndDepartmentData()
        {
            // Arrange
            IFixture fixture = new Fixture();

            var expectedDepartment = fixture.Build<Department>()
                .With(d => d.CargoSections, () => new List<CargoSection>())
                .With(d => d.Id, () => ObjectId.GenerateNewId().ToString())
                .With(d => d.LastSending, () => DateTime.Now.AddDays(-45))
                .With(d => d.NextSending, () => DateTime.Now.AddDays(+45))
                .Create();

            await dbContext.Departments.InsertOneAsync(expectedDepartment);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(AppRoutes.API_DEPARTMENT + "/" + expectedDepartment.Id);
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultData = System.Text.Json.JsonSerializer.Deserialize<Department>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedDepartment.DepartmentName, resultData.DepartmentName);
            Assert.AreEqual(expectedDepartment.DepartmentAddress, resultData.DepartmentAddress);
        }

        public void Dispose()
        {
            dbFixture.Dispose();
        }
    }
}
