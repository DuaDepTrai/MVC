using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRM2;

namespace HRM2.Controllers
{
    public class SEXesController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: SEXes
        public ActionResult Index()
        {
            return View(db.SEX.ToList());
        }

        // GET: SEXes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEX sEX = db.SEX.Find(id);
            if (sEX == null)
            {
                return HttpNotFound();
            }
            return View(sEX);
        }

        // GET: SEXes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SEXes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SexID,SexName")] SEX sEX)
        {
            if (ModelState.IsValid)
            {
                db.SEX.Add(sEX);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sEX);
        }

        // GET: SEXes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEX sEX = db.SEX.Find(id);
            if (sEX == null)
            {
                return HttpNotFound();
            }
            return View(sEX);
        }

        // POST: SEXes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SexID,SexName")] SEX sEX)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sEX).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sEX);
        }

        // GET: SEXes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SEX sEX = db.SEX.Find(id);
            if (sEX == null)
            {
                return HttpNotFound();
            }
            return View(sEX);
        }

        // POST: SEXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SEX sEX = db.SEX.Find(id);
            db.SEX.Remove(sEX);
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
