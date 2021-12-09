using System.ComponentModel.DataAnnotations;

namespace DockDelivery.Models.Department
{
    public class AssignDepartModel
    {
        [Required]
        public string DepartmentId { get; set; }
    }
}
