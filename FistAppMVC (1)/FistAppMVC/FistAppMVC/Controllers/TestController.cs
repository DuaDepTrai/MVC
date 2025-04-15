using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FistAppMVC.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            ViewBag.BayGio = DateTime.Now;
            TempData["name"]= "Nguyễn Văn Hiếu";
            return RedirectToAction("Contact","Home");
        }


        public ActionResult Hello(string name) 
        {
            ViewData["name"] = name;
            ViewData["CurrentTime"] = DateTime.Now;
            return View();
        }

        public ActionResult Dacbiet(int id) 
        {
            return Content("Day la tham so truyen vào: " + id);
        }
        public ActionResult Cong(float a, float b, float ? c)
        {
            float kq;
            if (c != null)
            {
                kq = a + b + c.GetValueOrDefault(0);
            }
            else {
                kq = a + b ;
            }
            return Content("Ket qua: "  + kq);
        }


    }
}