using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FistAppMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var ListStudent = new string[] { "Vũ Anh Kiệt", "Nguyễn Tiến Đạt", "Lê Cảnh Minh", "Nguyễn Tuấn Kiệt", "Bùi Đại Phú" };
            ViewBag.ListStudent = ListStudent;
            return View("");
        }

        // GET: Home
        public ActionResult About()
        {
            TempData["name"] = "Kieu Van Khai";
            return View();
        }

        public ActionResult Contact() 
        { 
            return View();
        }

    }
}