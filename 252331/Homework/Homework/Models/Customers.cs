using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Customers
    {
        [Required(ErrorMessage ="Customer ID is required")]
        public string CustomerID { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}