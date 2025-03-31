using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FistAppMVC.Models;
namespace FistAppMVC.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        // GET: Users
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
                    return Content("Đăng nhập thành công");
                }
                else
                {
                    return Content("Bạn sai UserName hoặc Password");
                }
            }

            return View();
        }
    }
}