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
    public class OrganizationsController : Controller
    {
        private HRMEntities db = new HRMEntities();

        // GET: Organizations
        public ActionResult Index()
        {
            return View(db.Organization.ToList());
        }

        // GET: Organizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organization.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationID,Code,OrganizationName,Address,Phone,Email")] Organization organization)
        {
            if (ModelState.IsValid)
            {

                if (db.Organization.Any(o => o.OrganizationName == organization.OrganizationName || o.Code == organization.Code))
                {
                    ModelState.AddModelError("Organization", "Organization Name or Organization Code already exists");
                    return View(organization);
                }
                db.Organization.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organization.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationID,Code,OrganizationName,Address,Phone,Email")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Organization.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var organization = db.Organization.Include(o => o.Employee)
                                       .FirstOrDefault(o => o.OrganizationID == id);

            if (organization == null)
            {
                return HttpNotFound();
            }

            // Nếu phòng ban có nhân viên → Không cho xóa
            if (organization.Employee.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete because this organization is using.";
                return RedirectToAction("Index");
            }

            db.Organization.Remove(organization);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Delete Organization successfully.";
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
