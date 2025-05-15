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
    public class MedicinesController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Medicines
        public ActionResult Index(string searchStr, string activeIngredientFilter, string categoryFilter, string manufacturerFilter, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter?.Trim();
            ViewBag.CategoryFilter = categoryFilter?.Trim();
            ViewBag.ManufacturerFilter = manufacturerFilter?.Trim();

            var medicines = db.Medicines.Include(m => m.Manufacturers).Include(m => m.MedicineCategories);

            // Populate dropdown filters
            ViewBag.ActiveIngredients = db.Medicines.Select(m => m.ActiveIngredient.Trim()).Distinct().OrderBy(x => x).ToList();
            ViewBag.Categories = db.MedicineCategories.Select(c => c.CategoryName.Trim()).Distinct().OrderBy(x => x).ToList();
            ViewBag.Manufacturers = db.Manufacturers.Select(m => m.Name.Trim()).Distinct().OrderBy(x => x).ToList();

            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                medicines = medicines.Where(m => m.TradeName.Contains(searchStr) || m.ActiveIngredient.Contains(searchStr) || m.Manufacturers.Name.Contains(searchStr));
            }
            if (!string.IsNullOrEmpty(activeIngredientFilter))
            {
                medicines = medicines.Where(m => m.ActiveIngredient == activeIngredientFilter);
            }
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                medicines = medicines.Where(m => m.MedicineCategories.CategoryName == categoryFilter);
            }
            if (!string.IsNullOrEmpty(manufacturerFilter))
            {
                medicines = medicines.Where(m => m.Manufacturers.Name == manufacturerFilter);
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = medicines.Count();
            var pagedMedicines = medicines.OrderBy(m => m.MedicineID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedMedicines);
        }

        // GET: Medicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }

        // GET: Medicines/Create
        public ActionResult Create()
        {
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name");
            ViewBag.CategoryID = new SelectList(db.MedicineCategories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicineID,TradeName,ActiveIngredient,CategoryID,Unit,Concentration,Form,SalePrice,ManufacturerID")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                if (db.Medicines.Any(m => m.TradeName == medicines.TradeName))
                {
                    ModelState.AddModelError("TradeName", "Trade Name is already used");
                    return View(medicines);
                }
                db.Medicines.Add(medicines);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", medicines.ManufacturerID);
            ViewBag.CategoryID = new SelectList(db.MedicineCategories, "CategoryID", "CategoryName", medicines.CategoryID);
            return View(medicines);
        }

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", medicines.ManufacturerID);
            ViewBag.CategoryID = new SelectList(db.MedicineCategories, "CategoryID", "CategoryName", medicines.CategoryID);
            return View(medicines);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicineID,TradeName,ActiveIngredient,CategoryID,Unit,Concentration,Form,SalePrice,ManufacturerID")] Medicines medicines)
        {
            if (ModelState.IsValid)
            {
                if (db.Medicines.Any(m => m.TradeName == medicines.TradeName && m.MedicineID != medicines.MedicineID))
                {
                    ModelState.AddModelError("TradeName", "Trade Name is already used");
                    return View(medicines);
                }
                db.Entry(medicines).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "Name", medicines.ManufacturerID);
            ViewBag.CategoryID = new SelectList(db.MedicineCategories, "CategoryID", "CategoryName", medicines.CategoryID);
            return View(medicines);
        }

        // GET: Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicines medicines = db.Medicines.Find(id);
            if (medicines == null)
            {
                return HttpNotFound();
            }
            return View(medicines);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicines medicines = db.Medicines.Find(id);
            if (db.Storages.Any(s => s.MedicineID == medicines.MedicineID))
            {
                ModelState.AddModelError("", "Unable to delete this medicine because it is currently used in storage data!");
                return View(medicines);
            }
            db.Medicines.Remove(medicines);
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
