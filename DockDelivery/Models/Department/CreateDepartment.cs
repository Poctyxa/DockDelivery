using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.Department
{
    public class CreateDepartment
    {
        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string DepartmentAddress { get; set; }
    }
}
