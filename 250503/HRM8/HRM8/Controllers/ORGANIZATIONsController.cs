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
    public class ORGANIZATIONsController : Controller
    {
        private HRMDBEntities db = new HRMDBEntities();

        // GET: ORGANIZATIONs
        public ActionResult Index(int page = 1, string sortOrder = "")
        {
            int sizePerPage = 5;
            int totalItems = db.ORGANIZATION.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            IQueryable<ORGANIZATION> orgQuery = db.ORGANIZATION;

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    orgQuery = orgQuery.OrderByDescending(o => o.OrganizationName);
                    break;
                case "name_asc":
                default:
                    orgQuery = orgQuery.OrderBy(o => o.OrganizationName);
                    break;
            }

            var list = orgQuery.Skip((page - 1) * sizePerPage)
                               .Take(sizePerPage)
                               .ToList();

            // Gán parent
            foreach (var item in list)
            {
                item.Parent = db.ORGANIZATION.FirstOrDefault(p => p.OrganizationID == item.ParentID);
            }

            return View(list);
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
                    ModelState.AddModelError("OrganizationName", "Organization Name is already used");
                    return View(oRGANIZATION);
                }
                if (db.ORGANIZATION.Any(o => o.Code == oRGANIZATION.Code))
                {
                    ModelState.AddModelError("Code", "Organization Code is already used");
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
                    ModelState.AddModelError("OrganizationName", "Organization Name is already used");
                    return View(oRGANIZATION);
                }
                if (db.ORGANIZATION.Any(o => o.Code == oRGANIZATION.Code && o.OrganizationID != oRGANIZATION.OrganizationID))
                {
                    ModelState.AddModelError("Code", "Organization Code is already used");
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
