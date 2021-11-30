using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.CargoSection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DockDelivery.Controllers
{
    [Route("api/[controller]")]
    public class CargoSectionController : Controller
    {
        private readonly ICargoSectionService cargoSectionService;

        public CargoSectionController(ICargoSectionService cargoSectionService)
        {
            this.cargoSectionService = cargoSectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cargoServices = await cargoSectionService.GetCargoSectionsAsync();
            return Ok(cargoServices);
        }

        [HttpGet]
        [Route("{cargoSectionId}")]
        public IActionResult Read(Guid cargoSectionId)
        {
            var cargoSection = cargoSectionService.GetByIdAsync(cargoSectionId).Result;
            return Ok(cargoSection);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCargoSection cargoSection)
        {
            if (ModelState.IsValid)
            {
                var newCargoSection = new CargoSection();
                newCargoSection.DepartmentId = cargoSection.DepartmentId;
                newCargoSection.CargoTypeId = cargoSection.CargoTypeId;
                newCargoSection.WeightLimit = cargoSection.WeightLimit;
                newCargoSection.CapacityLimit = cargoSection.CapacityLimit;

                await cargoSectionService.CreateAsync(newCargoSection);

                if (newCargoSection.Id != Guid.Empty)
                    return Ok("CargoSection was added");
            }

            return BadRequest("CargoSection was not added");
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] CargoSection cargoSection)
        {
            if (ModelState.IsValid)
            {
                await cargoSectionService.UpdateAsync(cargoSection);
                return Ok("CargoSection was updated");
            }

            return BadRequest("CargoSection was not updated");
        }


        [HttpDelete]
        [Route("{cargoSectionId}")]
        public async Task<IActionResult> Remove(Guid cargoSectionId)
        {
            if (ModelState.IsValid)
            {
                await cargoSectionService.RemoveAsync(cargoSectionId);
                return Ok("CargoSection was removed");
            }

            return BadRequest("CargoSection was not removed");
        }
    }
}
