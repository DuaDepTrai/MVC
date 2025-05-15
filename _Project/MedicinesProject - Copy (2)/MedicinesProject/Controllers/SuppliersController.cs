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
    public class SuppliersController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Suppliers
        public ActionResult Index(string searchStr, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;

            var suppliers = db.Suppliers.AsQueryable();

            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                suppliers = suppliers.Where(s => s.SupplierName.Contains(searchStr) || 
                                            s.PhoneNumber.Contains(searchStr) || 
                                            s.Email.Contains(searchStr));
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = suppliers.Count();
            var pagedSuppliers = suppliers.OrderBy(s => s.SupplierID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedSuppliers);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierID,SupplierName,Address,PhoneNumber,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                if (db.Suppliers.Any(s => s.SupplierName == suppliers.SupplierName))
                {
                    ModelState.AddModelError("SupplierName", "Supplier Name is already used");
                    return View(suppliers);
                }
                if (db.Suppliers.Any(s => s.PhoneNumber == suppliers.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone Number is already used");
                    return View(suppliers);
                }
                if (db.Suppliers.Any(s => s.Email == suppliers.Email))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(suppliers);
                }

                db.Suppliers.Add(suppliers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suppliers);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,SupplierName,Address,PhoneNumber,Email")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                if (db.Suppliers.Any(s => s.SupplierName == suppliers.SupplierName && s.SupplierID != suppliers.SupplierID))
                {
                    ModelState.AddModelError("SupplierName", "Supplier Name is already used");
                    return View(suppliers);
                }
                if (db.Suppliers.Any(s => s.PhoneNumber == suppliers.PhoneNumber && s.SupplierID != suppliers.SupplierID))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone Number is already used");
                    return View(suppliers);
                }
                if (db.Suppliers.Any(s => s.Email == suppliers.Email && s.SupplierID != suppliers.SupplierID))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(suppliers);
                }
                db.Entry(suppliers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suppliers suppliers = db.Suppliers.Find(id);
            if (suppliers == null)
            {
                return HttpNotFound();
            }
            return View(suppliers);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = db.Suppliers.Find(id);
            if (db.Storages.Any(s => s.SupplierID == suppliers.SupplierID))
            {
                ModelState.AddModelError("", "Unable to delete this supplier because it is currently used in storage data!");
                return View(suppliers);
            }

            db.Suppliers.Remove(suppliers);
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
