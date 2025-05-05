using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM4;

namespace HRM4.Controllers
{
    public class EMPLOYEEsController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: EMPLOYEEs
        public ActionResult Index(int? orgID)
        {
            var org = db.ORGANIZATION.ToList();
            ViewBag.Organizations = org;

            var eMPLOYEE = db.EMPLOYEE.Include(e => e.ORGANIZATION).Include(e => e.POSITION).Include(e => e.SEX);
            
            if (orgID.HasValue)
            {
                eMPLOYEE = eMPLOYEE.Where(e => e.OrganizationID == orgID.Value);
            }
            return View(eMPLOYEE.ToList());
        }

        // GET: EMPLOYEEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEE.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.ORGANIZATION, "OrganizationID", "OrganizationName");
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName");
            ViewBag.SexID = new SelectList(db.SEX, "SexID", "SexName");
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,Address,Phone,Mobile,Email,Photo,CurriculumVitae,CurriculumVitaeType,OrganizationID,DateofBirth,IndentifyNumber,PositionID,SexID")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.EMPLOYEE.Add(eMPLOYEE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.ORGANIZATION, "OrganizationID", "OrganizationName", eMPLOYEE.OrganizationID);
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.SexID = new SelectList(db.SEX, "SexID", "SexName", eMPLOYEE.SexID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEE.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.ORGANIZATION, "OrganizationID", "OrganizationName", eMPLOYEE.OrganizationID);
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.SexID = new SelectList(db.SEX, "SexID", "SexName", eMPLOYEE.SexID);
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,Address,Phone,Mobile,Email,Photo,CurriculumVitae,CurriculumVitaeType,OrganizationID,DateofBirth,IndentifyNumber,PositionID,SexID")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.ORGANIZATION, "OrganizationID", "OrganizationName", eMPLOYEE.OrganizationID);
            ViewBag.PositionID = new SelectList(db.POSITION, "PositionID", "PositionName", eMPLOYEE.PositionID);
            ViewBag.SexID = new SelectList(db.SEX, "SexID", "SexName", eMPLOYEE.SexID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEE.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EMPLOYEE eMPLOYEE = db.EMPLOYEE.Find(id);
            db.EMPLOYEE.Remove(eMPLOYEE);
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
