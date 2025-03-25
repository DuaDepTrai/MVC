using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _250324.App5.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return Content("Dua Dep Trai");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact() 
        {
            return View();
        }
    }
}