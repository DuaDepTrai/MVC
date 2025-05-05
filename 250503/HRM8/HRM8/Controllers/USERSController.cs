using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM8;

namespace HRM8.Controllers
{
    public class USERSController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: USERS
        public ActionResult Index()
        {
            ViewBag.IsAdmin = Session["UserName"] != null && Session["UserName"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase);

            var uSERS = db.USERS.Include(u => u.EMPLOYEE);
            return View(uSERS.ToList());
        }

        // GET: USERS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // GET: USERS/Create
        public ActionResult Create()
        {
            var Emp = db.EMPLOYEE.Select(e => new {EmployeeID = e.EmployeeID, FullName = e.FirstName + " " + e.LastName});
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName");
            return View();
        }

        // POST: USERS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsersID,UserName,Password,Discription,EmployeeID,ma_moi")] USERS uSERS)
        {

            var Emp = db.EMPLOYEE.Select(e => new { EmployeeID = e.EmployeeID, FullName = e.FirstName + " " + e.LastName });

            if (string.IsNullOrWhiteSpace(uSERS.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
                ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", uSERS.EmployeeID);
                return View(uSERS);
            }
            
            if (ModelState.IsValid)
            {
                if (db.USERS.Any(u => u.UserName == uSERS.UserName)) 
                {
                    ModelState.AddModelError("UserName", "UserName is already used");
                    return View(uSERS);
                }

                uSERS.Password = HashPassword(uSERS.Password);
                db.USERS.Add(uSERS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", uSERS.EmployeeID);
            return View(uSERS);
        }

        // GET: USERS/Edit/5
        public ActionResult Edit(int? id)
        {

            var Emp = db.EMPLOYEE.Select(e => new { EmployeeID = e.EmployeeID, FullName = e.FirstName + " " + e.LastName });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", uSERS.EmployeeID);
            return View(uSERS);
        }

        // POST: USERS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsersID,UserName,Password,Discription,EmployeeID,ma_moi")] USERS uSERS)
        {

            var Emp = db.EMPLOYEE.Select(e => new { EmployeeID = e.EmployeeID, FullName = e.FirstName + " " + e.LastName });

            if (ModelState.IsValid)
            {
                if (db.USERS.Any(u => u.UserName == uSERS.UserName && u.UsersID != uSERS.UsersID))
                {
                    ModelState.AddModelError("UserName", "UserName is already used");
                    return View(uSERS);
                }


                if (string.IsNullOrWhiteSpace(uSERS.Password))
                {
                    var oldUser = db.USERS.AsNoTracking().FirstOrDefault(u => u.UsersID == uSERS.UsersID);
                    if (oldUser != null)
                    {
                        uSERS.Password = oldUser.Password;
                    }
                }
                else
                {
                    uSERS.Password = HashPassword(uSERS.Password);
                }

                db.Entry(uSERS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", uSERS.EmployeeID);
            return View(uSERS);
        }

        // GET: USERS/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // POST: USERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            USERS uSERS = db.USERS.Find(id);
            db.USERS.Remove(uSERS);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            USERS users = new USERS();
            if (Request.Cookies["UserName"] != null) 
            {
                users.UserName = Request.Cookies["UserName"].Value;
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USERS users)
        {
            if (ModelState.IsValid) 
            {
                string hashPw = HashPassword(users.Password);
                var matchedUser = db.USERS.FirstOrDefault(u => u.UserName == users.UserName && u.Password == hashPw);
                if (matchedUser != null) 
                {
                    if (users.Remember)
                    {
                        HttpCookie cookie = new HttpCookie("UserName", users.UserName);
                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }

                    Session["UserID"] = users.UsersID;
                    Session["UserName"] = users.UserName;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View(users);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Users");
        }

        public string HashPassword(String pw)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create()) 
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(pw);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool IsAdmin()
        {
            return Session["UserName"] != null && Session["UserName"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
