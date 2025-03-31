using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _250328.AppProduct.Models;

namespace _250328.AppProduct.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users us)
        {
            if (us != null)
            {
                if (us.UserName == "admin" && us.Password == "123456") 
                {
                    return Content("Login Successfully");
                }
                else 
                {
                    return Content("Wrong UserName or Password");
                }
            }
            else
            {
                return Content("Please input UserName & Password");
            }
            return View();
        }
    }
}