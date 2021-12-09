using DockDelivery.Domain.Context;
using DockDelivery.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestDataLibrary;

namespace DockDelivery.Test.Controllers
{
    internal class DepartmentControllerTest
    {
        [Test]
        public async Task GetDepartments_SendRequest_ShouldReturnDepartments()
        {
            // Arrange

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<DockDeliveryDbContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<DockDeliveryDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("delivery_db");
                    });
                });
            });

            DockDeliveryDbContext dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<DockDeliveryDbContext>();
            List<Department> departments = new List<Department>() { new Department(), new Department(), new Department() };

            await dbContext.AddRangeAsync(departments);
            await dbContext.SaveChangesAsync();

            HttpClient httpClient = webHost.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("api/department");
            var resultContent = response.Content.ReadAsStringAsync().Result;

            var resultList = JsonSerializer.Deserialize<List<Department>>(resultContent);

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task AssignDepart_SendRequest_ShouldClearCargoAndChangeDepartmentDates()
        {
            // Arrange

            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<DockDeliveryDbContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<DockDeliveryDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("delivery_db");
                    });
                });
            });

            DockDeliveryDbContext dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<DockDeliveryDbContext>();
            Department department = DataCreator.CreateTestDepartment();

            await dbContext.Departments.AddAsync(department);
            await dbContext.SaveChangesAsync();

            HttpClient httpClient = webHost.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("api/department/assignDepart/" + department.Id);
            var resultContent = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Cargo is departed.", await response.Content.ReadAsStringAsync());
        }


        [Test]
        public async Task GetCapableDepartments_SendRequest_ShouldReturnDepartments()
        {
            // Arrange
            Department department = DataCreator.CreateTestDepartment();

            DateTime testDate = Convert.ToDateTime("2021-12-01");
            string testCargoTypeId = department.CargoSections.First().CargoType.Id;
            double testWeigth = 200;
            double testCapacity = 20;


            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<DockDeliveryDbContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<DockDeliveryDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("delivery_db");
                    });
                });
            });

            DockDeliveryDbContext dbContext = webHost.Services.CreateScope().ServiceProvider.GetService<DockDeliveryDbContext>();

            await dbContext.AddAsync(department);
            await dbContext.SaveChangesAsync();

            HttpClient httpClient = webHost.CreateClient();

            string query =
                "api/department/capable/dl=" + testDate.Date.ToString("dd-MM-yyyy") +
                "&w=" + testWeigth.ToString() +
                "&c=" + testCapacity.ToString() +
                "&t=" + testCargoTypeId.ToString();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(query);
            
            var resultContent = response.Content.ReadAsStringAsync().Result;
            var resultList = JsonSerializer.Deserialize<List<Department>>(resultContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, resultList.Count);
        }
    }
}
