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
    internal class CargoServiceTest
    {
        private List<Cargo> CreateTestCargos()
        {
            return new List<Cargo>() 
            { 
                new Cargo() { 
                    Id = Guid.NewGuid(),
                    Description = "Cargo 1", 
                    CargoSectionId = Guid.NewGuid(),
                    Capacity = 500,
                    Weight = 500
                    },
                new Cargo() {
                    Id = Guid.NewGuid(),
                    Description = "Cargo 2",
                    CargoSectionId = Guid.NewGuid(),
                    Capacity = 600,
                    Weight = 600
                    },
                new Cargo() {
                    Id = Guid.NewGuid(),
                    Description = "Cargo 3",
                    CargoSectionId = Guid.NewGuid(),
                    Capacity = 700,
                    Weight = 700
                    }
            };
        }

        [Test]
        public async Task GetCargosAsync_ShouldReturnCargosList()
        {
            // Arrange
            var cargos = CreateTestCargos();

            var cargoReposMoq = new Mock<ICargoRepository>();
            cargoReposMoq.Setup(rep => rep.GetAllAsync()).ReturnsAsync(cargos);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            var cargoService = new CargoService(uowMoq.Object);

            // Act
            var cargosResult = await cargoService.GetCargosAsync();

            // Assert
            Assert.NotNull(cargosResult);
            Assert.AreEqual(cargos.Count, cargosResult.ToList().Count);
            Assert.IsTrue(cargosResult.Contains(cargos[1]));
            Assert.IsFalse(cargosResult.Contains(new Cargo() { }));
        }

        [Test]
        public async Task GetCargoByIdAsync_ShouldReturnSingleCargo()
        {
            // Arrange
            var cargos = CreateTestCargos();
            var cargoId = cargos[1].Id;

            var cargoReposMoq = new Mock<ICargoRepository>();
            cargoReposMoq.Setup(rep => rep.GetByIdAsync(cargoId)).ReturnsAsync(cargos[1]);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            var cargoService = new CargoService(uowMoq.Object);

            // Act
            var cargoResult = await cargoService.GetByIdAsync(cargoId);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.AreEqual(cargos[1].Id, cargoResult.Id);
        }

        [Test]
        public async Task UpdateCargoAsync_ShouldReturnUpdatedCargo()
        {
            // Arrange
            List<Cargo> cargos = CreateTestCargos();
            string testOwner = "Vasil";
            string testDescription = "SuperCargo";

            Cargo expectedCargo = cargos[1];
            expectedCargo.Owner = testOwner;
            expectedCargo.Description = testDescription;

            var cargoReposMoq = new Mock<ICargoRepository>();
            cargoReposMoq.Setup(rep => rep.UpdateAsync(expectedCargo)).ReturnsAsync(expectedCargo);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            var cargoService = new CargoService(uowMoq.Object);

            // Act
            var cargoResult = await cargoService.UpdateAsync(expectedCargo);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.AreEqual(testOwner, cargoResult.Owner);
            Assert.AreEqual(testDescription, cargoResult.Description);
        }

        [Test]
        public async Task CreateCargoAsync_ShouldReturnCreatedCargo()
        {
            // Arrange
            string testOwner = "Vasil";
            string testDescription = "SuperCargo";
            double testCapacity = 500;
            double testWeight = 750;

            Cargo expectedCargo = new Cargo();
            expectedCargo.Owner = testOwner;
            expectedCargo.Description = testDescription;
            expectedCargo.Capacity = testCapacity;
            expectedCargo.Weight = testWeight;

            var cargoReposMoq = new Mock<ICargoRepository>();
            cargoReposMoq.Setup(rep => rep.CreateAsync(expectedCargo)).ReturnsAsync(expectedCargo);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            var cargoService = new CargoService(uowMoq.Object);

            // Act
            var cargoResult = await cargoService.CreateAsync(expectedCargo);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.IsTrue(cargoResult.Id != Guid.Empty);
            Assert.AreEqual(testDescription, cargoResult.Description);
            Assert.AreEqual(testOwner, cargoResult.Owner);
            Assert.AreEqual(testCapacity, cargoResult.Capacity);
            Assert.AreEqual(testWeight, cargoResult.Weight);
        }

        [Test]
        public async Task RemoveCargoAsync_ShouldReturnRemovedCargo()
        {
            // Arrange
            Guid testId = Guid.NewGuid();
            string testOwner = "Vasil";
            string testDescription = "SuperCargo";
            double testCapacity = 500;
            double testWeight = 750;

            Cargo expectedCargo = new Cargo();
            expectedCargo.Id = testId;
            expectedCargo.Owner = testOwner;
            expectedCargo.Description = testDescription;
            expectedCargo.Capacity = testCapacity;
            expectedCargo.Weight = testWeight;

            var cargoReposMoq = new Mock<ICargoRepository>();
            cargoReposMoq.Setup(rep => rep.RemoveAsync(testId)).ReturnsAsync(expectedCargo);

            var uowMoq = new Mock<IDockDeliveryUnitOfWork>();
            uowMoq.Setup(uow => uow.Cargos).Returns(cargoReposMoq.Object);

            var cargoService = new CargoService(uowMoq.Object);

            // Act
            var cargoResult = await cargoService.RemoveAsync(testId);

            // Assert
            Assert.NotNull(cargoResult);
            Assert.AreEqual(cargoResult.Id, testId);    
            Assert.AreEqual(testDescription, cargoResult.Description);
            Assert.AreEqual(testOwner, cargoResult.Owner);
            Assert.AreEqual(testCapacity, cargoResult.Capacity);
            Assert.AreEqual(testWeight, cargoResult.Weight);
        }
    }
}
