using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRMS;

namespace HRMS.Controllers
{
    public class EmployeesController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: Employees
        public ActionResult Index()
        {
            var employee = db.Employee.Include(e => e.Organization).Include(e => e.Position).Include(e => e.Sex);
            return View(employee.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organization, "OrganizationID", "Code");
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "PositionName");
            ViewBag.SexID = new SelectList(db.Sex, "SexID", "SexName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,Code,FirstName,LastName,Address,Phone,DateOfBirth,SexID,OrganizationID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organization, "OrganizationID", "Code", employee.OrganizationID);
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SexID = new SelectList(db.Sex, "SexID", "SexName", employee.SexID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organization, "OrganizationID", "Code", employee.OrganizationID);
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SexID = new SelectList(db.Sex, "SexID", "SexName", employee.SexID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,Code,FirstName,LastName,Address,Phone,DateOfBirth,SexID,OrganizationID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organization, "OrganizationID", "Code", employee.OrganizationID);
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "PositionName", employee.PositionID);
            ViewBag.SexID = new SelectList(db.Sex, "SexID", "SexName", employee.SexID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
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
    }
}
