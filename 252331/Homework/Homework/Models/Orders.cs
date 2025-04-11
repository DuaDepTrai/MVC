using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Orders
    {
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Customer is required")]
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Employee is required")]
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        [Required(ErrorMessage = "Shipper is required")]
        public int? ShipVia { get; set; }
        public string ShipperName { get; set; }
        public decimal Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }
}