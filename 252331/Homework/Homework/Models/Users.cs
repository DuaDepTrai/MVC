using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int EmployeeID { get; set; }
        public bool Remember { get; set; }
        public string FullName { get; set; }
    }
}