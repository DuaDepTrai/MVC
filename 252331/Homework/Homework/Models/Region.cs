using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        [Required(ErrorMessage = "Region Description is required!")]
        public string RegionDescription { get; set; }
    }
}