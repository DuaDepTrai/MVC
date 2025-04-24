using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace AppMVCEntity.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();
        // GET: Employees
        public ActionResult Index(int page = 1)
        {
            int sizePerPage = 6;
            int totalItems = db.Employees.Count();
            var employee = db.Employees.OrderBy(e => e.EmployeeID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            return View(employee.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {
            Employee employee = db.Employees.Find(id);

            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            //ViewBag.ReportsTo = new SelectList(db.Employees, "EmployeeID", "FirstName");
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.ReportsTo = new SelectList(Emp, "EmployeeID", "FullName");

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(Employee obj)
        {
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.ReportsTo = new SelectList(Emp, "EmployeeID", "FullName", obj.ReportsTo);

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Employees.Add(obj);
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

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = db.Employees.Find(id);
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.ReportsTo = new SelectList(Emp, "EmployeeID", "FullName", employee.ReportsTo);

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee obj)
        {
            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.ReportsTo = new SelectList(Emp, "EmployeeID", "FullName", obj.ReportsTo);
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Employees/Delete/5
        public ActionResult Delete(int id)
        {
            Employee employee = db.Employees.Find(id);

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Employee obj)
        {
            try
            {
                // TODO: Add delete logic here
                Employee emp = db.Employees.Find(id);
                db.Employees.Remove(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Show photo
        public FileContentResult GetPhoto(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null || employee.Photo == null)
            {
                string path = Server.MapPath("~/assets/images/default.jpg");
                byte[] defaultImage = System.IO.File.ReadAllBytes(path);
                return File(defaultImage, "image/jpeg");
            }
            return null;
        }
    }
}
