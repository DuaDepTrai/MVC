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
    public class SexesController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: Sexes
        public ActionResult Index()
        {
            return View(db.Sex.ToList());
        }

        // GET: Sexes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        // GET: Sexes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sexes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SexID,SexName")] Sex sex)
        {
            if (ModelState.IsValid)
            {
                db.Sex.Add(sex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sex);
        }

        // GET: Sexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        // POST: Sexes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SexID,SexName")] Sex sex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sex);
        }

        // GET: Sexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sex sex = db.Sex.Find(id);
            if (sex == null)
            {
                return HttpNotFound();
            }
            return View(sex);
        }

        // POST: Sexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sex sex = db.Sex.Find(id);
            db.Sex.Remove(sex);
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
