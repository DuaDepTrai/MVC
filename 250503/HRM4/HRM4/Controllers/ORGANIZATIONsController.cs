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
    public class ORGANIZATIONsController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: ORGANIZATIONs
        public ActionResult Index()
        {
            return View(db.ORGANIZATION.ToList());
        }

        // GET: ORGANIZATIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORGANIZATION oRGANIZATION = db.ORGANIZATION.Find(id);
            if (oRGANIZATION == null)
            {
                return HttpNotFound();
            }
            return View(oRGANIZATION);
        }

        // GET: ORGANIZATIONs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ORGANIZATIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationID,OrganizationName,Code,Address,Phone,Fax,Email,Website,ParentID,Lever")] ORGANIZATION oRGANIZATION)
        {
            if (ModelState.IsValid)
            {
                if (db.ORGANIZATION.Any(o => o.OrganizationName == oRGANIZATION.OrganizationName))
                {
                    ModelState.AddModelError("OrganizationName", "Organization Name is already exists");
                    return View(oRGANIZATION);
                }
                if (db.ORGANIZATION.Any(o => o.Code == oRGANIZATION.Code))
                {
                    ModelState.AddModelError("Code", "Organization Code is already exists");
                    return View(oRGANIZATION);
                }
                db.ORGANIZATION.Add(oRGANIZATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oRGANIZATION);
        }

        // GET: ORGANIZATIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORGANIZATION oRGANIZATION = db.ORGANIZATION.Find(id);
            if (oRGANIZATION == null)
            {
                return HttpNotFound();
            }
            return View(oRGANIZATION);
        }

        // POST: ORGANIZATIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationID,OrganizationName,Code,Address,Phone,Fax,Email,Website,ParentID,Lever")] ORGANIZATION oRGANIZATION)
        {
            if (ModelState.IsValid)
            {
                if (db.ORGANIZATION.Any(o => o.OrganizationName == oRGANIZATION.OrganizationName && o.OrganizationID != oRGANIZATION.OrganizationID))
                {
                    ModelState.AddModelError("OrganizationName", "Organization Name is already exists");
                    return View(oRGANIZATION);
                }
                if (db.ORGANIZATION.Any(o => o.Code == oRGANIZATION.Code && o.OrganizationID != oRGANIZATION.OrganizationID))
                {
                    ModelState.AddModelError("Code", "Organization Code is already exists");
                    return View(oRGANIZATION);
                }
                db.Entry(oRGANIZATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oRGANIZATION);
        }

        // GET: ORGANIZATIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORGANIZATION oRGANIZATION = db.ORGANIZATION.Find(id);
            if (oRGANIZATION == null)
            {
                return HttpNotFound();
            }
            return View(oRGANIZATION);
        }

        // POST: ORGANIZATIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORGANIZATION oRGANIZATION = db.ORGANIZATION.Find(id);
            if (db.EMPLOYEE.Any(e => e.OrganizationID == oRGANIZATION.OrganizationID))
            {
                ModelState.AddModelError("", "Organization is being used, cannot remove");
                return View(oRGANIZATION);
            }
            db.ORGANIZATION.Remove(oRGANIZATION);
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
