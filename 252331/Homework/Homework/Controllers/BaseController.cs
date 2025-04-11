//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Homework.Controllers
//{
//    public class BaseController : Controller
//    {
//        // GET: Base
//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
//            string action = filterContext.ActionDescriptor.ActionName;

//            if (Session["UserID"] == null && !(controller == "Users" && action == "Login"))
//            {
//                // Nếu chưa đăng nhập, chuyển hướng về trang Login
//                filterContext.Result = new RedirectResult("~/Users/Login");
//                return;
//            }

//            base.OnActionExecuting(filterContext);
//        }
//    }
//}