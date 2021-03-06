using DockDelivery.Business.Abstract;
using DockDelivery.Domain.Entities;
using DockDelivery.Models.Department;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DockDelivery.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet]
        [Route("{departmentId}")]
        public IActionResult Read(string departmentId)
        {
            var department = departmentService.GetByIdAsync(departmentId).Result;
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartment department)
        {
            if (ModelState.IsValid) { 
                var newDepartment = new Department();
                newDepartment.DepartmentName = department.DepartmentName;
                newDepartment.DepartmentAddress = department.DepartmentAddress;
                newDepartment.LastSending = department.LastSending;
                newDepartment.NextSending = department.NextSending;

                await departmentService.CreateAsync(newDepartment);
            
                if (newDepartment.Id.Length > 0)
                return Ok("Department was added");
            }

            return BadRequest("Department was not added");
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.UpdateAsync(department);
                return Ok("Department was updated");
            }

            return BadRequest("Department was not updated");
        }

        [HttpDelete]
        [Route("{departmentId}")]
        public async Task<IActionResult> Remove(string departmentId)
        {
            if (ModelState.IsValid)
            {
                await departmentService.RemoveAsync(departmentId);
                return Ok("Department was removed");
            }

            return BadRequest("Department was not removed");
        }

        [HttpPost]
        [Route("assignDepart")]
        public async Task<IActionResult> AssignDepart([FromBody] AssignDepartModel departmentInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model was not valid");

            try
            {
                Department department = await departmentService.GetByIdAsync(departmentInfo.DepartmentId);

                // some date VALIDATION
                if (department.NextSending == null)
                {
                    return BadRequest("Chose date for first");
                } 
                else if (
                    department.LastSending != null && 
                    department.NextSending != null &&
                    department.NextSending < department.LastSending)
                {
                    return BadRequest("NextSending date cannot be faster then LastSending");
                }

                department.LastSending = department.NextSending;
                department.NextSending = null;

                bool cleared = await departmentService.ClearSectionsAsync(department);

                if (!cleared)
                    throw new Exception("Error at removing cargos.");

                await departmentService.UpdateAsync(department);

                return Ok("Cargo is departed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("capable/dl={dateLimit}&w={weight}&c={capacity}&t={cargoTypeId}")]
        public async Task<IActionResult> GetCapableDepartments(
            DateTime dateLimit,
            string cargoTypeId,
            double weight, 
            double capacity)
        {
            try
            {
                List<Department> departments = 
                    (await departmentService.GetCapableDepartmentsAsync(cargoTypeId, weight, capacity, dateLimit))
                    .ToList();

                foreach(var department in departments)
                {
                    department.CargoSections = new List<CargoSection>();
                }

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
