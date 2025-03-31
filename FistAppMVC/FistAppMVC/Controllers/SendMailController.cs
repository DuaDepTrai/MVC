using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FistAppMVC.Controllers
{
    public class SendMailController : Controller
    {
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection fr)
        {
            string kq = "";

            kq = "Mail to: " + fr["txtTo"] + "<br />";
            kq = kq+ " Subject: " + fr["txtSubject"] + "<br />";

            kq = kq + " Content: " + fr["txtContent"] + "<br />";

            return Content(kq);
        }

    }
}