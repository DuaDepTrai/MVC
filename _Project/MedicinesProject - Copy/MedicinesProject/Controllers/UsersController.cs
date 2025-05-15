using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicinesProject;

namespace MedicinesProject.Controllers
{
    public class UsersController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Users
        public ActionResult Index(string searchStr, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;

            var users = db.Users.AsQueryable();

            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                users = users.Where(u => u.UserName.Contains(searchStr) ||
                                            u.FullName.Contains(searchStr) ||
                                            u.Phone.Contains(searchStr) ||
                                            u.Email.Contains(searchStr));
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = users.Count();
            var pagedUsers = users.OrderBy(u => u.UserID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedUsers);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,FullName,Phone,Address,Email")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.UserName == users.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName is already used");
                    return View(users);
                }
                if (db.Users.Any(u => u.Phone == users.Phone))
                {
                    ModelState.AddModelError("Phone", "Phone number is already used");
                    return View(users);
                }
                if (db.Users.Any(u => u.Email == users.Email))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(users);
                }

                users.Password = HashPassword(users.Password);

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,FullName,Phone,Address,Email")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.UserName == users.UserName && u.UserID != users.UserID))
                {
                    ModelState.AddModelError("UserName", "UserName is already used");
                    return View(users);
                }
                if (db.Users.Any(u => u.Phone == users.Phone && u.UserID != users.UserID))
                {
                    ModelState.AddModelError("Phone", "Phone number is already used");
                    return View(users);
                }
                if (db.Users.Any(u => u.Email == users.Email && u.UserID != users.UserID))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(users);
                }

                if (string.IsNullOrWhiteSpace(users.Password))
                {
                    var oldUser = db.Users.AsNoTracking().FirstOrDefault(u => u.UserID == users.UserID);
                    if (oldUser != null)
                    {
                        users.Password = oldUser.Password;
                    }
                }
                else
                {
                    users.Password = HashPassword(users.Password);
                }

                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
            Users users = new Users();
            if (Request.Cookies["UserName"] != null)
            {
                users.UserName = Request.Cookies["UserName"].Value;
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users users)
        {
            if (ModelState.IsValid)
            {
                string hashPw = HashPassword(users.Password);
                var matchedUser = db.Users.FirstOrDefault(u => u.UserName == users.UserName && u.Password == hashPw);
                if (matchedUser != null)
                {
                    if (users.Remember)
                    {
                        HttpCookie cookie = new HttpCookie("UserName", users.UserName);
                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }

                    Session["UserID"] = matchedUser.UserID;
                    Session["UserName"] = matchedUser.UserName;
                    Session["FullName"] = matchedUser.FullName;

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
    }
}
