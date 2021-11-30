using DockDelivery.Business.Service;
using DockDelivery.Domain.Entities;
using DockDelivery.Domain.Repositories.Abstract;
using DockDelivery.Domain.UoW;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockDelivery.Test.Services
{
    internal class CargoTypeServiceTest
    {
        private List<CargoType> CreateTestCargoTypes()
        {
            return new List<CargoType>()
            {
                new CargoType() {
                    Id = Guid.NewGuid(),
                    TypeName = "CargoType 1"
                    },
                new CargoType() {
                    Id = Guid.NewGuid(),
                    TypeName = "CargoType 2"
                    },
                new CargoType() {
                    Id = Guid.NewGuid(),
                    TypeName = "CargoType 3"
                    }
            };
        }

        [Test]
        public async Task GetCargoTypesAsync_ShouldReturnCargoTypesList()
        {
            // Arrange
            var cargoTypes = CreateTestCargoTypes();

            var cargoTypeReposMoq = new Mock<ICargoTypeRepository>();
            cargoTypeReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargoTypes);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoTypes).Returns(cargoTypeReposMoq.Object);

            var cargoTypeService = new CargoTypeService(uowMoq.Object);

            // Act
            var cargoTypesResult = await cargoTypeService.GetCargoTypesAsync();

            // Assert
            Assert.NotNull(cargoTypesResult);
            Assert.AreEqual(cargoTypes.Count, cargoTypesResult.ToList().Count);
            Assert.IsTrue(cargoTypesResult.Contains(cargoTypes[1]));
            Assert.IsFalse(cargoTypesResult.Contains(new CargoType() { }));
        }

        [Test]
        public async Task GetCargoByIdAsync_ShouldReturnSingleCargo()
        {
            // Arrange
            var cargoTypes = CreateTestCargoTypes();
            var cargoTypeId = cargoTypes[1].Id;

            var cargoTypeReposMoq = new Mock<ICargoTypeRepository>();
            cargoTypeReposMoq.Setup(rep => rep.GetByIdAsync(cargoTypeId)).ReturnsAsync(cargoTypes[1]);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoTypes).Returns(cargoTypeReposMoq.Object);

            var cargoTypeService = new CargoTypeService(uowMoq.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.GetByIdAsync(cargoTypeId);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.AreEqual(cargoTypes[1].Id, cargoTypeResult.Id);
        }

        [Test]
        public async Task UpdateCargoTypeAsync_ShouldReturnUpdatedCargoType()
        {
            // Arrange
            List<CargoType> cargoTypes = CreateTestCargoTypes();
            string testName = "Test Cargo";

            CargoType expectedCargoType = cargoTypes[1];
            expectedCargoType.TypeName = testName;

            var cargoTypeReposMoq = new Mock<ICargoTypeRepository>();
            cargoTypeReposMoq.Setup(rep => rep.UpdateAsync(expectedCargoType)).ReturnsAsync(expectedCargoType);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoTypes).Returns(cargoTypeReposMoq.Object);

            var cargoTypeService = new CargoTypeService(uowMoq.Object);

            // Act

            var cargoTypeResult = await cargoTypeService.UpdateAsync(expectedCargoType);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.AreEqual(testName, cargoTypeResult.TypeName);
        }

        [Test]
        public async Task CreateCargoAsync_ShouldReturnCreatedCargo()
        {
            // Arrange
            string testTypeName = "expected type name";

            CargoType expectedCargoType = new CargoType();
            expectedCargoType.TypeName = testTypeName;

            var cargoTypeReposMoq = new Mock<ICargoTypeRepository>();
            cargoTypeReposMoq.Setup(rep => rep.CreateAsync(expectedCargoType)).ReturnsAsync(expectedCargoType);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoTypes).Returns(cargoTypeReposMoq.Object);

            var cargoTypeService = new CargoTypeService(uowMoq.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.CreateAsync(expectedCargoType);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.IsTrue(cargoTypeResult.Id != Guid.Empty);
            Assert.AreEqual(testTypeName, cargoTypeResult.TypeName);
        }

        [Test]
        public async Task RemoveCargoTypeAsync_ShouldReturnRemovedCargoType()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            string testTypeName = "test type name";

            CargoType expectedCargoType = new CargoType();
            expectedCargoType.Id = testId;
            expectedCargoType.TypeName = testTypeName;

            var cargoTypeReposMoq = new Mock<ICargoTypeRepository>();
            cargoTypeReposMoq.Setup(rep => rep.RemoveAsync(testId)).ReturnsAsync(expectedCargoType);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.CargoTypes).Returns(cargoTypeReposMoq.Object);

            var cargoTypeService = new CargoTypeService(uowMoq.Object);

            // Act
            var cargoTypeResult = await cargoTypeService.RemoveAsync(testId);

            // Assert
            Assert.NotNull(cargoTypeResult);
            Assert.AreEqual(cargoTypeResult.Id, testId);
            Assert.AreEqual(cargoTypeResult.TypeName, expectedCargoType.TypeName);
        }
    }
}
