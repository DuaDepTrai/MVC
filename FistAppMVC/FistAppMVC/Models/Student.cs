﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FistAppMVC.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public bool Sex { get; set; }
        public int Status { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
    }
}