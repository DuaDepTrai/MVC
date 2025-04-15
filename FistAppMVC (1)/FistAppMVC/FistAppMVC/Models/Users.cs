using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FistAppMVC.Models
{
    public class Users
    {
        public int UserID { get; set; }
        [Required(ErrorMessage ="UserName bắt buộc phải có")]
        [DisplayName("Tên User")]
        
        public string UserName { get; set; }
        [StringLength(20,MinimumLength =8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string cfPassword { get; set; }
        [Required(ErrorMessage = "Description bắt buộc phải có")]
        public string Description { get; set; }
        
        public int EmployeeID {  get; set; }
        public bool Remember { get; set; }
        [DisplayName("Nhân viên")]
        public string FullName { get; set; }
    }
}