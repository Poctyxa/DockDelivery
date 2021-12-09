using DockDelivery.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDataLibrary
{
    // After implementing Fixtures class is useless
    // Not removed becous of legacy tests
    public static class DataCreator
    {
        public static Department CreateTestDepartment()
        {
            string departmentId = ObjectId.GenerateNewId().ToString();
            string cargoType1Id = ObjectId.GenerateNewId().ToString();
            string cargoType2Id = ObjectId.GenerateNewId().ToString();
            string section1Id = ObjectId.GenerateNewId().ToString();
            string section2Id = ObjectId.GenerateNewId().ToString();
            string cargo1Id = ObjectId.GenerateNewId().ToString();
            string cargo2Id = ObjectId.GenerateNewId().ToString();
            string cargo3Id = ObjectId.GenerateNewId().ToString();
            string cargo4Id = ObjectId.GenerateNewId().ToString();
            string cargo5Id = ObjectId.GenerateNewId().ToString();

            // Dapertment

            Department department = new Department()
            {
                Id = departmentId,
                DepartmentAddress = "department test address",
                DepartmentName = "department test name",
                LastSending = Convert.ToDateTime("2021-08-01"),
                NextSending = Convert.ToDateTime("2021-10-01"),
            };

            // CargoType

            CargoType cargoType1 = new CargoType()
            {
                Id = cargoType1Id,
                TypeName = "Cars"
            };

            CargoType cargoType2 = new CargoType()
            {
                Id = cargoType2Id,
                TypeName = "Technics"
            };

            // CargoSection

            CargoSection cargoSection1 = new CargoSection()
            {
                Id = section1Id,
                CapacityLimit = 1000,
                DepartmentId = departmentId,
                Department = department,
                CargoTypeId = cargoType1Id,
                CargoType = cargoType1,
                WeightLimit = 5000
            };

            CargoSection cargoSection2 = new CargoSection()
            {
                Id = section2Id,
                CapacityLimit = 2000,
                DepartmentId = departmentId,
                Department = department,
                CargoTypeId = cargoType2Id,
                CargoType = cargoType2,
                WeightLimit = 8000
            };

            // Cargo

            Cargo cargo1 = new Cargo()
            {
                Id = cargo1Id,
                CargoSection = cargoSection1,
                CargoSectionId = section1Id,
                Owner = "Vasyl",
                Description = "Opel vectra",
                Capacity = 6,
                Weight = 2
            };

            Cargo cargo2 = new Cargo()
            {
                Id = cargo2Id,
                CargoSection = cargoSection1,
                CargoSectionId = section1Id,
                Owner = "Petya",
                Description = "Mazda 6",
                Capacity = 6,
                Weight = 1.6
            };

            Cargo cargo3 = new Cargo()
            {
                Id = cargo3Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Igor",
                Description = "Lenovo xCool 5000",
                Capacity = 1,
                Weight = 0.002
            };

            Cargo cargo4 = new Cargo()
            {
                Id = cargo4Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Vlad",
                Description = "HP Zbook 4",
                Capacity = 1,
                Weight = 0.0025
            };

            Cargo cargo5 = new Cargo()
            {
                Id = cargo5Id,
                CargoSection = cargoSection2,
                CargoSectionId = section2Id,
                Owner = "Rost",
                Description = "Asus Dream 12",
                Capacity = 1,
                Weight = 0.0018
            };

            // Arrange

            List<CargoSection> sections = new List<CargoSection>() { cargoSection1, cargoSection2 };
            List<Cargo> cargos1 = new List<Cargo>() { cargo1, cargo2 };
            List<Cargo> cargos2 = new List<Cargo>() { cargo3, cargo4, cargo5 };

            cargoSection1.Cargos = cargos1;
            cargoSection2.Cargos = cargos2;

            department.CargoSections = sections;

            return department;
        }

        public static List<CargoSection> CreateTestCargoSections()
        {
            return new List<CargoSection>()
            {
                new CargoSection() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentId= ObjectId.GenerateNewId().ToString(),
                    CargoTypeId = ObjectId.GenerateNewId().ToString(),
                    CapacityLimit = 500,
                    WeightLimit = 500
                    },
                new CargoSection() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentId= ObjectId.GenerateNewId().ToString(),
                    CargoTypeId = ObjectId.GenerateNewId().ToString(),
                    CapacityLimit = 600,
                    WeightLimit = 600
                    },
                new CargoSection() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentId = ObjectId.GenerateNewId().ToString(),
                    CargoTypeId = ObjectId.GenerateNewId().ToString(),
                    CapacityLimit = 700,
                    WeightLimit = 700
                    }
            };
        }

        public static List<Cargo> CreateTestCargos()
        {
            return new List<Cargo>()
            {
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 1",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 50,
                    Weight = 44
                    },
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 2",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 33,
                    Weight = 22
                    },
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 3",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 44,
                    Weight = 55
                    },
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 4",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 22,
                    Weight = 33
                    },
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 5",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 66,
                    Weight = 55
                    },
                new Cargo() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Cargo 6",
                    CargoSectionId = ObjectId.GenerateNewId().ToString(),
                    Capacity = 11,
                    Weight = 14
                    }
            };
        }

        public static List<CargoType> CreateTestCargoTypes()
        {
            return new List<CargoType>()
            {
                new CargoType() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    TypeName = "CargoType 1"
                    },
                new CargoType() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    TypeName = "CargoType 2"
                    },
                new CargoType() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    TypeName = "CargoType 3"
                    }
            };
        }

        public static List<Department> CreateTestDepartments()
        {
            return new List<Department>()
            {
                new Department() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentName = "Department 1",
                    DepartmentAddress = "Test address 1",
                    LastSending = Convert.ToDateTime("2021-08-01"),
                    NextSending = Convert.ToDateTime("2021-10-01")
                    },
                new Department() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentName = "Department 2",
                    DepartmentAddress = "Test address 2",
                    LastSending = Convert.ToDateTime("2021-09-01"),
                    NextSending = Convert.ToDateTime("2021-11-01")
                    },
                new Department() {
                    Id = ObjectId.GenerateNewId().ToString(),
                    DepartmentName = "Department 3",
                    DepartmentAddress = "Test address 3",
                    LastSending = Convert.ToDateTime("2021-10-01"),
                    NextSending = Convert.ToDateTime("2021-12-01")
                    }
            };
        }
    }
}
