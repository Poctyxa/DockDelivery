using AutoFixture;
using AutoFixture.AutoMoq;
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
    public class CargoTypeServiceTest
    {
        [Theory, AutoMoqData]
        public async Task GetCargoTypesAsync_ShouldReturnCargoTypesList(
            IFixture fixture, 
            Mock<ICargoTypeRepository> cargoTypeRepository,
            [Frozen]Mock<IMongoDockDeliveryUnitOfWork> uowMock,
            MongoCargoTypeService cargoTypeService
            )
        {
            // Arrange
            List<CargoType> cargoTypes = fixture.CreateMany<CargoType>(15).ToList();
            cargoTypeRepository.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargoTypes);
            uowMock.Setup(uow => uow.CargoTypes).Returns(cargoTypeRepository.Object);

            // Act
            var cargoTypesResult = await cargoTypeService.GetCargoTypesAsync();

            // Assert
            Assert.NotNull(cargoTypesResult);
            Assert.NotEmpty(cargoTypesResult.ToList());
            Assert.Contains(cargoTypes[1], cargoTypesResult);
            Assert.DoesNotContain(new CargoType() { }, cargoTypesResult);
        }

        [Theory, AutoMoqData]
        public async Task GetCargoByIdAsync_ShouldReturnSingleCargo(
            IFixture fixture,
            Mock<ICargoTypeRepository> cargoTypeRepository,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMock,
            MongoCargoTypeService cargoTypeService)
        {
            // Arrange
            CargoType cargoType = fixture.Create<CargoType>();
            var cargoTypeId = cargoType.Id;

            cargoTypeRepository.Setup(rep => rep.GetByIdAsync(cargoTypeId)).ReturnsAsync(cargoType);
            uowMock.Setup(uow => uow.CargoTypes).Returns(cargoTypeRepository.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.GetByIdAsync(cargoTypeId);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.Equal(cargoTypeId, cargoTypeResult.Id);
        }

        [Theory, AutoMoqData]
        public async Task UpdateCargoTypeAsync_ShouldReturnUpdatedCargoType(
            IFixture fixture,
            Mock<ICargoTypeRepository> cargoTypeRepository,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMock,
            MongoCargoTypeService cargoTypeService)
        {
            // Arrange
            string testName = "Test Cargo";

            CargoType expectedCargoType = fixture.Create<CargoType>();
            expectedCargoType.TypeName = testName;

            cargoTypeRepository.Setup(rep => rep.UpdateAsync(expectedCargoType)).ReturnsAsync(expectedCargoType);
            uowMock.Setup(uow => uow.CargoTypes).Returns(cargoTypeRepository.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.UpdateAsync(expectedCargoType);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.Equal(testName, cargoTypeResult.TypeName);
        }

        [Theory, AutoMoqData]
        public async Task CreateCargoAsync_ShouldReturnCreatedCargo(
            IFixture fixture,
            Mock<ICargoTypeRepository> cargoTypeRepository,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMock,
            MongoCargoTypeService cargoTypeService)
        {
            // Arrange
            string testTypeName = "expected type name";

            CargoType expectedCargoType = fixture.Create<CargoType>();
            expectedCargoType.TypeName = testTypeName;

            cargoTypeRepository.Setup(rep => rep.CreateAsync(expectedCargoType)).ReturnsAsync(expectedCargoType);
            uowMock.Setup(uow => uow.CargoTypes).Returns(cargoTypeRepository.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.CreateAsync(expectedCargoType);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.Equal(testTypeName, cargoTypeResult.TypeName);
        }

        [Theory, AutoMoqData]
        public async Task RemoveCargoTypeAsync_ShouldReturnRemovedCargoType(
            IFixture fixture,
            Mock<ICargoTypeRepository> cargoTypeRepository,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMock,
            MongoCargoTypeService cargoTypeService)
        {
            // Arrange
            CargoType expectedCargoType = fixture.Create<CargoType>();

            cargoTypeRepository.Setup(rep => rep.RemoveAsync(expectedCargoType.Id)).ReturnsAsync(expectedCargoType);
            uowMock.Setup(uow => uow.CargoTypes).Returns(cargoTypeRepository.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.RemoveAsync(expectedCargoType.Id);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.Equal(cargoTypeResult.Id, expectedCargoType.Id);
            Assert.Equal(cargoTypeResult.TypeName, expectedCargoType.TypeName);
        }
    }
}
