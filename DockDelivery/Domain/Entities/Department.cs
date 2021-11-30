using System;
using System.Collections.Generic;

namespace DockDelivery.Domain.Entities
{
    public class Department : EntityBase
    {
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public DateTime? LastSending { get; set; }
        public DateTime? NextSending { get; set; }
        public virtual IEnumerable<CargoSection> CargoSections { get; set; }
    }
}
