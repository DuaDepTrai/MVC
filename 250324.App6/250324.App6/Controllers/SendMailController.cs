using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _250324.App6.Controllers
{
    public class SendMailController : Controller
    {
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string kq = "";
            kq = "Mail to: " + formCollection["txtTo"] + "<br/>";
            kq = kq + "Subject: " + formCollection["txtSub"] + "<br/>";
            kq = kq + "Content: " + formCollection["txtContent"] + "<br/>";
            return Content(kq);
        }
    }
}