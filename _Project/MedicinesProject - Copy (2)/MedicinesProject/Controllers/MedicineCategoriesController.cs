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
    public class MedicineCategoriesController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: MedicineCategories
        public ActionResult Index(string searchStr, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;

            var cates = db.MedicineCategories.AsQueryable();

            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                cates = cates.Where(c => c.CategoryName.Contains(searchStr));
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = cates.Count();
            var pagedCates = cates.OrderBy(m => m.CategoryID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedCates);
        }

        // GET: MedicineCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineCategories medicineCategories = db.MedicineCategories.Find(id);
            if (medicineCategories == null)
            {
                return HttpNotFound();
            }
            return View(medicineCategories);
        }

        // GET: MedicineCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicineCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] MedicineCategories medicineCategories)
        {
            if (ModelState.IsValid)
            {
                if (db.MedicineCategories.Any(m => m.CategoryName == medicineCategories.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "Category Name is already used");
                    return View(medicineCategories);
                }
                db.MedicineCategories.Add(medicineCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicineCategories);
        }

        // GET: MedicineCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineCategories medicineCategories = db.MedicineCategories.Find(id);
            if (medicineCategories == null)
            {
                return HttpNotFound();
            }
            return View(medicineCategories);
        }

        // POST: MedicineCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] MedicineCategories medicineCategories)
        {
            if (ModelState.IsValid)
            {
                if (db.MedicineCategories.Any(m => m.CategoryName == medicineCategories.CategoryName && m.CategoryID != medicineCategories.CategoryID))
                {
                    ModelState.AddModelError("CategoryName", "Category Name is already used");
                    return View(medicineCategories);
                }
                db.Entry(medicineCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicineCategories);
        }

        // GET: MedicineCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineCategories medicineCategories = db.MedicineCategories.Find(id);
            if (medicineCategories == null)
            {
                return HttpNotFound();
            }
            return View(medicineCategories);
        }

        // POST: MedicineCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicineCategories medicineCategories = db.MedicineCategories.Find(id);
            if (db.Medicines.Any(m => m.CategoryID == medicineCategories.CategoryID))
            {
                ModelState.AddModelError("", "Unable to delete this category because it is currently used in medicines data!");
                return View(medicineCategories);
            }
            db.MedicineCategories.Remove(medicineCategories);
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
