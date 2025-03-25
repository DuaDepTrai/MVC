using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _250324.App5.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return RedirectToAction("Contact", "Home");
        }

        public ActionResult Hello(string name) 
        { 
            return Content("Dua Hello " + name);
        }

        public ActionResult Speciall(int id) 
        {
            return Content("This is: " + id);
        }

        public ActionResult Sum(float a, float b, float ? c) 
        {
            if (c.HasValue) { return Content($"Sum ({a} + {b} + {c}) = {a + b + c}"); }
            else { return Content($"Sum ({a} + {b}) = {a + b}"); }
        }
    }
}