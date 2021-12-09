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
    public class CargoSectionServiceTest
    {
        [Theory, AutoMoqData]
        public async Task GetCargoSectionsAsync_ShouldReturnCargoSectionsList(
            IFixture fixture,
            Mock<ICargoSectionRepository> cargoSectionReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoSectionService cargoSectionService
            )
        {
            // Arrange
            List<CargoSection> cargoSections = fixture.Build<CargoSection>()
                .Without(d => d.Department)
                .Without(d => d.Cargos)
                .CreateMany<CargoSection>(15)
                .ToList();

            cargoSectionReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargoSections);
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            // Act
            var cargoSectionsResult = await cargoSectionService.GetCargoSectionsAsync();

            // Assert
            Assert.NotNull(cargoSectionsResult);
            Assert.Equal(cargoSections.Count, cargoSectionsResult.ToList().Count);
            Assert.Contains(cargoSections[1], cargoSectionsResult);
            Assert.DoesNotContain(new CargoSection() { }, cargoSectionsResult);
        }

        [Theory, AutoMoqData]
        public async Task GetCargoSectionByIdAsync_ShouldReturnSingleCargoSection(
            IFixture fixture,
            Mock<ICargoSectionRepository> cargoSectionReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoSectionService cargoSectionService)
        {
            // Arrange
            var cargoSection = fixture.Build<CargoSection>()
                .Without(d => d.Department)
                .Without(d => d.Cargos)
                .Create<CargoSection>();
            var cargoSectionId = cargoSection.Id;

            cargoSectionReposMoq.Setup(rep => rep.GetByIdAsync(cargoSectionId)).ReturnsAsync(cargoSection);
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.GetByIdAsync(cargoSectionId);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.Equal(cargoSection.Id, cargoSectionResult.Id);
        }

        [Theory, AutoMoqData]
        public async Task UpdateCargoSectionByIdAsync_ShouldReturnUpdatedCargoSection(
            IFixture fixture,
            Mock<ICargoSectionRepository> cargoSectionReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoSectionService cargoSectionService)
        {
            // Arrange

            CargoSection expectedCargoSection = fixture.Build<CargoSection>()
                .Without(d => d.Department)
                .Without(d => d.Cargos)
                .Create<CargoSection>();

            cargoSectionReposMoq.Setup(rep => rep.UpdateAsync(expectedCargoSection)).ReturnsAsync(expectedCargoSection);
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.UpdateAsync(expectedCargoSection);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.Equal(expectedCargoSection.CapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.Equal(expectedCargoSection.WeightLimit, cargoSectionResult.WeightLimit);
        }

        [Theory, AutoMoqData]
        public async Task CreateCargoSectionByIdAsync_ShouldReturnCreatedCargoSection(
            IFixture fixture,
            Mock<ICargoSectionRepository> cargoSectionReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoSectionService cargoSectionService)
        {
            // Arrange
            CargoSection expectedCargoSection = fixture.Build<CargoSection>()
                .Without(d => d.Department)
                .Without(d => d.Cargos)
                .Create<CargoSection>();

            cargoSectionReposMoq.Setup(rep => rep.CreateAsync(expectedCargoSection)).ReturnsAsync(expectedCargoSection);
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.CreateAsync(expectedCargoSection);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.Equal(expectedCargoSection.CapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.Equal(expectedCargoSection.WeightLimit, cargoSectionResult.WeightLimit);
        }

        [Theory, AutoMoqData]
        public async Task RemoveCargoSectionByIdAsync_ShouldReturnRemovedCargoSection(
            IFixture fixture,
            Mock<ICargoSectionRepository> cargoSectionReposMoq,
            [Frozen] Mock<IMongoDockDeliveryUnitOfWork> uowMoq,
            MongoCargoSectionService cargoSectionService)
        {
            // Arrange
            CargoSection expectedCargoSection = fixture.Build<CargoSection>()
                .Without(d => d.Department)
                .Without(d => d.Cargos)
                .Create<CargoSection>();

            cargoSectionReposMoq.Setup(rep => rep.RemoveAsync(expectedCargoSection.Id)).ReturnsAsync(expectedCargoSection);
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.RemoveAsync(expectedCargoSection.Id);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.Equal(expectedCargoSection.Id, cargoSectionResult.Id);
            Assert.Equal(expectedCargoSection.CapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.Equal(expectedCargoSection.WeightLimit, cargoSectionResult.WeightLimit);
        }
    }
}
