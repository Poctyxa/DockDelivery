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
    public class CargoServiceTest
    {
        [Theory, AutoMoqData]
        public async Task GetCargosAsync_ShouldReturnCargosList(
            IFixture fixture,
            Mock<ICargoRepository> cargoReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoService cargoService)
        {
            // Arrange
            var cargos = fixture.Build<Cargo>().Without(d => d.CargoSection).CreateMany<Cargo>(15).ToList();

            cargoReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargos);
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            // Act
            var cargosResult = await cargoService.GetCargosAsync();

            // Assert
            Assert.NotNull(cargosResult);
            Assert.Contains(cargos[1], cargosResult);
            Assert.DoesNotContain(new Cargo(), cargosResult);
        }

        [Theory, AutoMoqData]
        public async Task GetCargoByIdAsync_ShouldReturnSingleCargo(
            IFixture fixture,
            Mock<ICargoRepository> cargoReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoService cargoService)
        {
            // Arrange
            Cargo expectedCargo = fixture.Build<Cargo>().Without(d => d.CargoSection).Create<Cargo>();

            cargoReposMoq.Setup(rep => rep.GetByIdAsync(expectedCargo.Id)).ReturnsAsync(expectedCargo);
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            // Act
            var cargoResult = await cargoService.GetByIdAsync(expectedCargo.Id);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.Equal(expectedCargo.Id, cargoResult.Id);
        }

        [Theory, AutoMoqData]
        public async Task UpdateCargoAsync_ShouldReturnUpdatedCargo(
            IFixture fixture,
            Mock<ICargoRepository> cargoReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoService cargoService)
        {
            // Arrange
            Cargo expectedCargo = fixture.Build<Cargo>().Without(d => d.CargoSection).Create<Cargo>();

            cargoReposMoq.Setup(rep => rep.UpdateAsync(expectedCargo)).ReturnsAsync(expectedCargo);
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            // Act
            var cargoResult = await cargoService.UpdateAsync(expectedCargo);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.Equal(expectedCargo.Owner, cargoResult.Owner);
            Assert.Equal(expectedCargo.Description, cargoResult.Description);
            Assert.Equal(expectedCargo.Capacity, cargoResult.Capacity);
            Assert.Equal(expectedCargo.Weight, cargoResult.Weight);
        }

        [Theory, AutoMoqData]
        public async Task CreateCargoAsync_ShouldReturnCreatedCargo(
            IFixture fixture,
            Mock<ICargoRepository> cargoReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoService cargoService)
        {
            // Arrange
            Cargo expectedCargo = fixture.Build<Cargo>().Without(d => d.CargoSection).Create<Cargo>();

            cargoReposMoq.Setup(rep => rep.CreateAsync(expectedCargo)).ReturnsAsync(expectedCargo);
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            // Act
            var cargoResult = await cargoService.CreateAsync(expectedCargo);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.True(cargoResult.Id.Length > 0);
            Assert.Equal(expectedCargo.Description, cargoResult.Description);
            Assert.Equal(expectedCargo.Owner, cargoResult.Owner);
            Assert.Equal(expectedCargo.Capacity, cargoResult.Capacity);
            Assert.Equal(expectedCargo.Weight, cargoResult.Weight);
        }

        [Theory, AutoMoqData]
        public async Task RemoveCargoAsync_ShouldReturnRemovedCargo(
            IFixture fixture,
            Mock<ICargoRepository> cargoReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoService cargoService)
        {
            // Arrange
            Cargo expectedCargo = fixture.Build<Cargo>().Without(d => d.CargoSection).Create<Cargo>();

            cargoReposMoq.Setup(rep => rep.RemoveAsync(expectedCargo.Id)).ReturnsAsync(expectedCargo);
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            // Act
            var cargoResult = await cargoService.RemoveAsync(expectedCargo.Id);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.Equal(cargoResult.Id, expectedCargo.Id);
            Assert.Equal(expectedCargo.Description, cargoResult.Description);
            Assert.Equal(expectedCargo.Owner, cargoResult.Owner);
            Assert.Equal(expectedCargo.Capacity, cargoResult.Capacity);
            Assert.Equal(expectedCargo.Weight, cargoResult.Weight);
        }
    }
}
