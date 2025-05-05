using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class UsersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();
        // GET: Users
        public ActionResult Index()
        {
            return View(db.USERS.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            USER user = db.USERS.Find(id);
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(USER obj)
        {
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.USERS.Add(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(obj);
            }
            catch
            {
                return View(obj);
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            USER user = db.USERS.Find(id);
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", user.EmployeeID);

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, USER obj)
        {
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified; db.SaveChanges();
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            USER user = db.USERS.Find(id);
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, USER obj)
        {
            try
            {
                // TODO: Add delete logic here
                USER user = db.USERS.Find(id);
                db.USERS.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users - LOGIN
        public ActionResult Login()
        {
            USER users = new USER();

            if (Request.Cookies["UserName"] != null)
            {
                users.UserName = Request.Cookies["UserName"].Value;
            }

            return View(users);
        }

        //POST: Users - LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER user)
        {
            if (ModelState.IsValid)
            {
                var matchedUser = db.USERS
                    .FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (matchedUser != null)
                {            
                    // Ghi cookie nếu nhớ đăng nhập
                    if (user.Remember)
                    {
                        HttpCookie cookie = new HttpCookie("UserName", user.UserName);
                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }

                    // Lưu session

                    Session["UserID"] = matchedUser.UserID;
                    Session["UserName"] = matchedUser.UserName;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(user);

        }

        //LOGOUT:
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Users");
        }

        public string hashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create()) 
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
