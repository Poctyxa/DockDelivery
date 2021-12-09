using AutoFixture;
using AutoFixture.Xunit2;
using DockDelivery.Business.Service.Mongo;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.UoW.Mongo;
using MongoDB.Bson;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDataLibrary;
using Xunit;

namespace DockDelivery.Mongo.Unit.Test
{
    public class DepartmentServiceTest
    {
        [Theory, AutoMoqData]
        public async Task GetDepartmentsAsync_ShouldReturnDepartmentsList(
            IFixture fixture,
            Mock<IDepartmentRepository> departmentReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoDepartmentService departmentService)
        {
            // Arrange
            var departments = fixture.Build<Department>().Without(d => d.CargoSections).CreateMany(12).ToList();

            departmentReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(departments);
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            // Act
            var departmentsResult = (await departmentService.GetDepartmentsAsync()).ToList();

            // Assert
            Assert.NotNull(departmentsResult);
            Assert.Contains(departments[1], departmentsResult);
            Assert.DoesNotContain(new Department(), departmentsResult);
        }

        [Theory, AutoMoqData]
        public async Task GetDepartmentByIdAsync_ShouldReturnSingleDepartment(
            IFixture fixture,
            Mock<IDepartmentRepository> departmentReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoDepartmentService departmentService)
        {
            // Arrange
            Department expectedDepartment = fixture.Build<Department>().Without(d => d.CargoSections).Create();

            departmentReposMoq.Setup(rep => rep.GetByIdAsync(expectedDepartment.Id)).ReturnsAsync(expectedDepartment);
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            // Act
            var departmentResult = await departmentService.GetByIdAsync(expectedDepartment.Id);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.Equal(expectedDepartment.Id, departmentResult.Id);
        }

        [Theory, AutoMoqData]
        public async Task UpdateDepartmentAsync_ShouldReturnUpdatedDepartment(
            IFixture fixture,
            Mock<IDepartmentRepository> departmentReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoDepartmentService departmentService)
        {
            // Arrange
            Department expectedDepartment = fixture.Build<Department>().Without(d => d.CargoSections).Create<Department>();

            departmentReposMoq.Setup(rep => rep.UpdateAsync(expectedDepartment)).ReturnsAsync(expectedDepartment);
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            // Act
            var departmentResult = await departmentService.UpdateAsync(expectedDepartment);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.Equal(expectedDepartment.DepartmentName, departmentResult.DepartmentName);
            Assert.Equal(expectedDepartment.DepartmentAddress, departmentResult.DepartmentAddress);
        }

        [Theory, AutoMoqData]
        public async Task CreateDepartmentAsync_ShouldReturnCreatedDepartment(
            IFixture fixture,
            Mock<IDepartmentRepository> departmentReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoDepartmentService departmentService)
        {
            // Arrange
            Department expectedDepartment = fixture.Build<Department>().Without(d => d.CargoSections).Create<Department>();

            departmentReposMoq.Setup(rep => rep.CreateAsync(expectedDepartment)).ReturnsAsync(expectedDepartment);
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            // Act
            var departmentResult = await departmentService.CreateAsync(expectedDepartment);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.True(departmentResult.Id.Length > 0);
            Assert.Equal(expectedDepartment.DepartmentName, departmentResult.DepartmentName);
            Assert.Equal(expectedDepartment.DepartmentAddress, departmentResult.DepartmentAddress);
        }

        [Theory, AutoMoqData]
        public async Task RemoveDepartmentAsync_ShouldReturnRemovedDepartment(
            IFixture fixture,
            Mock<IDepartmentRepository> departmentReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoDepartmentService departmentService)
        {
            // Arrange
            Department expectedDepartment = fixture.Build<Department>().Without(d => d.CargoSections).Create<Department>();

            departmentReposMoq.Setup(rep => rep.RemoveAsync(expectedDepartment.Id)).ReturnsAsync(expectedDepartment);
            uowMoq.Setup(uow => uow.Departments).Returns(departmentReposMoq.Object);

            // Act
            var departmentResult = await departmentService.RemoveAsync(expectedDepartment.Id);

            // Assert
            Assert.NotNull(departmentResult);
            Assert.Equal(expectedDepartment.Id, departmentResult.Id);
            Assert.Equal(expectedDepartment.DepartmentName, departmentResult.DepartmentName);
            Assert.Equal(expectedDepartment.DepartmentAddress, departmentResult.DepartmentAddress);
        }
    }
}
