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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequiredDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ShippedDate { get; set; }
        [Required(ErrorMessage = "Shipper is required")]
        public int? ShipVia { get; set; }
        public string ShipperName { get; set; }
        [Required(ErrorMessage = "Freight is required")]
        public decimal Freight { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string ShipName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string ShipAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string ShipCity { get; set; }
        [Required(ErrorMessage = "Region is required")]
        public string ShipRegion { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public string ShipPostalCode { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string ShipCountry { get; set; }
    }
}