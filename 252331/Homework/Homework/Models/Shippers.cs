using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Shippers
    {
        public int ShipperID { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone {  get; set; }
    }
}