using System;
using System.Collections.Generic;
using System.Text;

namespace TestDataLibrary
{
    public static class AppRoutes
    {
        public static string API_DEPARTMENT { get; } = "api/department"; 
        public static string ASSIGN_DEPART { get; } = "/assignDepart";
        public static string CAPABLE_DEPARTMENTS(DateTime maxDate, double cargoWeight, double cargoCapacity, string cargoTypeId)
        {
            return "/capable/dl=" + maxDate.Date.ToString("dd-MM-yyyy") +
                "&w=" + cargoWeight.ToString() +
                "&c=" + cargoCapacity.ToString() +
                "&t=" + cargoTypeId;
        }

        public static string API_CARGO_TYPE { get; } = "api/cargoType";
        public static string API_CARGO_SECTION { get; } = "api/cargoSection";
        public static string API_CARGO { get; } = "api/cargo";
    }
}
