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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DockDelivery.Test.Controllers
{
    internal class DepartmentControllerTest
    {
        private Department CreateTestDepartment()
        {
            Guid departmentId = Guid.NewGuid();
            Guid cargoType1Id = Guid.NewGuid();
            Guid cargoType2Id = Guid.NewGuid();
            Guid section1Id = Guid.NewGuid();
            Guid section2Id = Guid.NewGuid();
            Guid cargo1Id = Guid.NewGuid();
            Guid cargo2Id = Guid.NewGuid();
            Guid cargo3Id = Guid.NewGuid();
            Guid cargo4Id = Guid.NewGuid();
            Guid cargo5Id = Guid.NewGuid();

            // Dapertment

            Department department = new Department()
            {
                Id = departmentId,
                DepartmentAddress = "department test address",
                DepartmentName = "department test name",
                LastSending = Convert.ToDateTime("2021-08-01"),
                NextSending = Convert.ToDateTime("2021-10-01"),
            };

            // CargoType

            CargoType cargoType1 = new CargoType()
            {
                Id = cargoType1Id,
                TypeName = "Cars"
            };

            CargoType cargoType2 = new CargoType()
            {
                Id = cargoType2Id,
                TypeName = "Technics"
            };

            // CargoSection

            CargoSection cargoSection1 = new CargoSection()
            {
                Id = section1Id,
                CapacityLimit = 1000,
                DepartmentId = departmentId,
                Department = department,
                CargoTypeId = cargoType1Id,
                CargoType = cargoType1,
                WeightLimit = 5000
            };

            CargoSection cargoSection2 = new CargoSection()
            {
                Id = section2Id,
                CapacityLimit = 2000,
                DepartmentId = departmentId,
                Department = department,
                CargoTypeId = cargoType2Id,
                CargoType = cargoType2,
                WeightLimit = 8000
            };

            // Cargo

            Cargo cargo1 = new Cargo()
            {
                Id = cargo1Id,
                CargoSection = cargoSection1,
                CargoSectionId = section1Id,
                Owner = "Vasyl",
                Description = "Opel vectra",
                Capacity = 6,
                Weight = 2
            };

            Cargo cargo2 = new Cargo()
            {
                Id = cargo2Id,
                CargoSection = cargoSection1,
                CargoSectionId = section1Id,
                Owner = "Petya",
                Description = "Mazda 6",
                Capacity = 6,
                Weight = 1.6
            };

            Cargo cargo3 = new Cargo()
            {
                Id = cargo3Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Igor",
                Description = "Lenovo xCool 5000",
                Capacity = 1,
                Weight = 0.002
            };

            Cargo cargo4 = new Cargo()
            {
                Id = cargo4Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Vlad",
                Description = "HP Zbook 4",
                Capacity = 1,
                Weight = 0.0025
            };

            Cargo cargo5 = new Cargo()
            {
                Id = cargo5Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Rost",
                Description = "Asus Dream 12",
                Capacity = 1,
                Weight = 0.0018
            };

            // Arrange

            List<CargoSection> sections = new List<CargoSection>() { cargoSection1, cargoSection2 };
            List<Cargo> cargos1 = new List<Cargo>() { cargo1, cargo2};
            List<Cargo> cargos2 = new List<Cargo>() { cargo3, cargo4, cargo5 };

            cargoSection1.Cargos = cargos1;
            cargoSection2.Cargos = cargos2;

            department.CargoSections = sections;

            return department;
        }

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
            Assert.AreEqual(departments.Count, resultList.Count);
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
            Department department = CreateTestDepartment();

            await dbContext.AddAsync(department);
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
            Department department = CreateTestDepartment();

            DateTime testDate = Convert.ToDateTime("2021-12-01");
            Guid testCargoTypeId = department.CargoSections.First().CargoType.Id;
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
                "&s=" + testCargoTypeId.ToString() +
                "&w=" + testWeigth.ToString() +
                "&c=" + testCapacity.ToString();

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
