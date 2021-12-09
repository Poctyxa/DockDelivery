using DockDelivery.Business.Service;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.UoW;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataLibrary;

namespace DockDelivery.Test.Services
{
    internal class CargoSectionServiceTest
    {
        [Test]
        public async Task GetCargoSectionsAsync_ShouldReturnCargoSectionsList()
        {
            // Arrange
            var cargoSections = DataCreator.CreateTestCargoSections();

            var cargoSectionReposMoq = new Mock<ICargoSectionRepository>();
            cargoSectionReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargoSections);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            var cargoSectionService = new CargoSectionService(uowMoq.Object);

            // Act
            var cargoSectionsResult = await cargoSectionService.GetCargoSectionsAsync();

            // Assert
            Assert.NotNull(cargoSectionsResult);
            Assert.AreEqual(cargoSections.Count, cargoSectionsResult.ToList().Count);
            Assert.IsTrue(cargoSectionsResult.Contains(cargoSections[1]));
            Assert.IsFalse(cargoSectionsResult.Contains(new CargoSection() { }));
        }

        [Test]
        public async Task GetCargoSectionByIdAsync_ShouldReturnSingleCargoSection()
        {
            // Arrange
            var cargoSections = DataCreator.CreateTestCargoSections();
            var cargoSectionId = cargoSections[1].Id;

            var cargoSectionReposMoq = new Mock<ICargoSectionRepository>();
            cargoSectionReposMoq.Setup(rep => rep.GetByIdAsync(cargoSectionId)).ReturnsAsync(cargoSections[1]);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            var cargoSectionService = new CargoSectionService(uowMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.GetByIdAsync(cargoSectionId);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.AreEqual(cargoSections[1].Id, cargoSectionResult.Id);
        }

        [Test]
        public async Task UpdateCargoSectionByIdAsync_ShouldReturnUpdatedCargoSection()
        {
            // Arrange
            List<CargoSection> cargoSections = DataCreator.CreateTestCargoSections();
            double testCapacityLimit = 800.5;
            double testWeightLimit = 676.33;

            CargoSection expectedCargoSection = cargoSections[1];
            expectedCargoSection.CapacityLimit = testCapacityLimit;
            expectedCargoSection.WeightLimit = testWeightLimit;

            var cargoSectionReposMoq = new Mock<ICargoSectionRepository>();
            cargoSectionReposMoq.Setup(rep => rep.UpdateAsync(expectedCargoSection)).ReturnsAsync(expectedCargoSection);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            var cargoSectionService = new CargoSectionService(uowMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.UpdateAsync(expectedCargoSection);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.AreEqual(testCapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.AreEqual(testWeightLimit, cargoSectionResult.WeightLimit);
        }

        [Test]
        public async Task CreateCargoSectionByIdAsync_ShouldReturnCreatedCargoSection()
        {
            // Arrange
            double testCapacityLimit = 500;
            double testWeightLimit = 750;

            CargoSection expectedCargoSection = new CargoSection();
            expectedCargoSection.CapacityLimit = testCapacityLimit;
            expectedCargoSection.WeightLimit = testWeightLimit;

            var cargoSectionReposMoq = new Mock<ICargoSectionRepository>();
            cargoSectionReposMoq.Setup(rep => rep.CreateAsync(expectedCargoSection)).ReturnsAsync(expectedCargoSection);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            var cargoSectionService = new CargoSectionService(uowMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.CreateAsync(expectedCargoSection);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.IsTrue(cargoSectionResult.Id.Length > 0);
            Assert.AreEqual(testCapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.AreEqual(testWeightLimit, cargoSectionResult.WeightLimit);
        }

        [Test]
        public async Task RemoveCargoSectionByIdAsync_ShouldReturnRemovedCargoSection()
        {
            // Arrange
            string testId = ObjectId.GenerateNewId().ToString();
            double testCapacityLimit = 500;
            double testWeightLimit = 750;

            CargoSection expectedCargoSection = new CargoSection();
            expectedCargoSection.Id = testId;
            expectedCargoSection.CapacityLimit = testCapacityLimit;
            expectedCargoSection.WeightLimit = testWeightLimit;

            var cargoSectionReposMoq = new Mock<ICargoSectionRepository>();
            cargoSectionReposMoq.Setup(rep => rep.RemoveAsync(testId)).ReturnsAsync(expectedCargoSection);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoSections).Returns(cargoSectionReposMoq.Object);

            var cargoSectionService = new CargoSectionService(uowMoq.Object);

            // Act
            var cargoSectionResult = await cargoSectionService.RemoveAsync(testId);

            // Assert
            Assert.NotNull(cargoSectionResult);
            Assert.AreEqual(cargoSectionResult.Id, testId);
            Assert.AreEqual(testCapacityLimit, cargoSectionResult.CapacityLimit);
            Assert.AreEqual(testWeightLimit, cargoSectionResult.WeightLimit);
        }
    }
}
