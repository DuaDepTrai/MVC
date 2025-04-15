using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FistAppMVC.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        [Required(ErrorMessage ="Tên vùng bắt buộc phải có")]
        [DisplayName("Tên vùng")]
        public string RegionDescription { get; set; }
    }
}