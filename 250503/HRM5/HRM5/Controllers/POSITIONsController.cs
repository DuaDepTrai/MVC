using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM5;

namespace HRM5.Controllers
{
    public class POSITIONsController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: POSITIONs
        public ActionResult Index()
        {
            return View(db.POSITION.ToList());
        }

        // GET: POSITIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITION.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            return View(pOSITION);
        }

        // GET: POSITIONs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: POSITIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionID,PositionName")] POSITION pOSITION)
        {
            if (ModelState.IsValid)
            {
                db.POSITION.Add(pOSITION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pOSITION);
        }

        // GET: POSITIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITION.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            return View(pOSITION);
        }

        // POST: POSITIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionID,PositionName")] POSITION pOSITION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pOSITION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pOSITION);
        }

        // GET: POSITIONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            POSITION pOSITION = db.POSITION.Find(id);
            if (pOSITION == null)
            {
                return HttpNotFound();
            }
            return View(pOSITION);
        }

        // POST: POSITIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            POSITION pOSITION = db.POSITION.Find(id);
            db.POSITION.Remove(pOSITION);
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
