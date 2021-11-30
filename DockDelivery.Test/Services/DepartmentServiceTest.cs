using DockDelivery.Business.Service;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.UoW;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockDelivery.Test.Services
{
    internal class DepartmentServiceTest
    {
        private List<Department> CreateTestDepartments()
        {
            return new List<Department>()
            {
                new Department() {
                    Id = Guid.NewGuid(),
                    DepartmentName = "Department 1",
                    DepartmentAddress = "Test address 1"
                    },
                new Department() {
                    Id = Guid.NewGuid(),
                    DepartmentName = "Department 2",
                    DepartmentAddress = "Test address 2"
                    },
                new Department() {
                    Id = Guid.NewGuid(),
                    DepartmentName = "Department 3",
                    DepartmentAddress = "Test address 3"
                    }
            };
        }

        [Test]
        public async Task GetDepartmentsAsync_ShouldReturnDepartmentsList()
        {
            // Arrange
            var departments = CreateTestDepartments();

            var departmentReposMoq = new Mock<IDepartmentRepository>();
            departmentReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(departments);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            var departmentService = new DepartmentService(uowMoq.Object);

            // Act
            var departmentsResult = await departmentService.GetDepartmentsAsync();

            // Assert
            Assert.NotNull(departmentsResult);
            Assert.AreEqual(departments.Count, departmentsResult.ToList().Count);
            Assert.IsTrue(departmentsResult.Contains(departments[1]));
            Assert.IsFalse(departmentsResult.Contains(new Department() { }));
        }

        [Test]
        public async Task GetDepartmentByIdAsync_ShouldReturnSingleDepartment()
        {
            // Arrange
            var departments = CreateTestDepartments();
            var departmentId = departments[1].Id;

            var departmentReposMoq = new Mock<IDepartmentRepository>();
            departmentReposMoq.Setup(rep => rep.GetByIdAsync(departmentId)).ReturnsAsync(departments[1]);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            var departmentService = new DepartmentService(uowMoq.Object);

            // Act
            var departmentResult = await departmentService.GetByIdAsync(departmentId);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.AreEqual(departments[1].Id, departmentResult.Id);
        }

        [Test]
        public async Task UpdateDepartmentAsync_ShouldReturnUpdatedDepartment()
        {
            // Arrange
            List<Department> departments = CreateTestDepartments();
            string testName = "Test department name";
            string testAddress = "Test department address";

            Department expectedDepartment = departments[1];
            expectedDepartment.DepartmentName = testName;
            expectedDepartment.DepartmentAddress = testAddress;

            var departmentReposMoq = new Mock<IDepartmentRepository>();
            departmentReposMoq.Setup(rep => rep.UpdateAsync(expectedDepartment)).ReturnsAsync(expectedDepartment);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            var departmentService = new DepartmentService(uowMoq.Object);

            // Act
            var departmentResult = await departmentService.UpdateAsync(expectedDepartment);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.AreEqual(testName, departmentResult.DepartmentName);
            Assert.AreEqual(testAddress, departmentResult.DepartmentAddress);
        }

        [Test]
        public async Task CreateDepartmentAsync_ShouldReturnCreatedDepartment()
        {
            // Arrange
            string testName = "Test department name";
            string testAddress = "Test department address";

            Department expectedDepartment = new Department();
            expectedDepartment.DepartmentName = testName;
            expectedDepartment.DepartmentAddress = testAddress;

            var departmentReposMoq = new Mock<IDepartmentRepository>();
            departmentReposMoq.Setup(rep => rep.CreateAsync(expectedDepartment)).ReturnsAsync(expectedDepartment);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            var departmentService = new DepartmentService(uowMoq.Object);

            // Act
            var departmentResult = await departmentService.CreateAsync(expectedDepartment);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.IsTrue(departmentResult.Id != Guid.Empty);
            Assert.AreEqual(testName, departmentResult.DepartmentName);
            Assert.AreEqual(testAddress, departmentResult.DepartmentAddress);
        }

        [Test]
        public async Task RemoveDepartmentAsync_ShouldReturnRemovedDepartment()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            string testName = "Test department name";
            string testAddress = "Test department address";

            Department expectedDepartment = new Department();
            expectedDepartment.Id = testId;
            expectedDepartment.DepartmentName = testName;
            expectedDepartment.DepartmentAddress = testAddress;

            var departmentReposMoq = new Mock<IDepartmentRepository>();
            departmentReposMoq.Setup(rep => rep.RemoveAsync(testId)).ReturnsAsync(expectedDepartment);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            var departmentService = new DepartmentService(uowMoq.Object);

            // Act
            var departmentResult = await departmentService.RemoveAsync(testId);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.AreEqual(departmentResult.Id, testId);
            Assert.AreEqual(testName, departmentResult.DepartmentName);
            Assert.AreEqual(testAddress, departmentResult.DepartmentAddress);
        }
    }
}
